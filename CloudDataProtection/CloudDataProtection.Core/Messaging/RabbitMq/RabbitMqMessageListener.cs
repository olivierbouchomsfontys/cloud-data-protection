using System;
using System.Threading;
using System.Threading.Tasks;
using CloudDataProtection.Core.Messaging.RabbitMq.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CloudDataProtection.Core.Messaging.RabbitMq
{
    public abstract class RabbitMqMessageListener<TModel> : BackgroundService, IMessageListener<TModel> where TModel : class
    {
        private readonly ILogger<RabbitMqMessageListener<TModel>> _logger;
        private readonly RabbitMqConfiguration _configuration;

        protected abstract string RoutingKey { get; }
        protected abstract string Queue { get; }

        private ConnectionFactory _connectionFactory;
        private ConnectionFactory ConnectionFactory
        {
            get
            {
                if (_connectionFactory == null)
                {
                    _connectionFactory = new ConnectionFactory
                    {
                        HostName = _configuration.Hostname,
                        Port = _configuration.Port,
                        UserName = _configuration.UserName,
                        Password = _configuration.Password,
                        VirtualHost = _configuration.VirtualHost
                    };
                }
                
                return _connectionFactory;
            }
        }

        private IConnection _connection;
        private IConnection Connection => _connection ??= ConnectionFactory.CreateConnection();

        private IModel _channel; 

        protected RabbitMqMessageListener(IOptions<RabbitMqConfiguration> options, ILogger<RabbitMqMessageListener<TModel>> logger)
        {
            _logger = logger;
            _configuration = options.Value;
            
            Init();
        }
        
        public abstract Task HandleMessage(TModel model);
        
        protected sealed override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            EventingBasicConsumer consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (_ ,args) => await HandleMessage(args);

            _channel.BasicConsume(string.Empty, false, consumer);

            return Task.CompletedTask;
        }

        public sealed override Task StopAsync(CancellationToken cancellationToken)
        {
            _connection?.Close();
            
            return base.StopAsync(cancellationToken);
        }

        private async Task HandleMessage(BasicDeliverEventArgs args)
        {
            if (args.RoutingKey != RoutingKey)
            {
                return;
            }
            
            TModel model = args.GetModel<TModel>();
         
            _logger.LogInformation("Handling message with subject {GetSubject} and model {Model}", args.RoutingKey, model);

            await HandleMessage(model);
            
            _channel.BasicAck(args.DeliveryTag, false);
        }

        private void Init()
        {
            Policy
                .Handle<Exception>()
                .WaitAndRetry(_configuration.RetryCount, GetRetryDelay, OnInitRetry)
                .Execute(DoInit);
        }

        private void OnInitRetry(Exception exception, TimeSpan timeSpan, int attempt, Context context)
        {
            _logger.LogWarning(exception, "An error occurred during initialization");
            _logger.LogWarning("Retrying initialization, attempt nr. {Attempt} / {RetryCount}", attempt, _configuration.RetryCount);
        }
        
        private TimeSpan GetRetryDelay(int attempt)
        {
            return TimeSpan.FromMilliseconds(attempt * _configuration.RetryDelayMs);
        }

        private void DoInit()
        {
            _channel = Connection.CreateModel();
            _channel.ExchangeDeclare(_configuration.Exchange, ExchangeType.Fanout, true);
            
            // A queue should not be automatically deleted and should survive a broker restart
            _channel.QueueDeclare(Queue, exclusive: false, durable: true, autoDelete: false);
            _channel.QueueBind(Queue, _configuration.Exchange, RoutingKey);
        }
    }
}

