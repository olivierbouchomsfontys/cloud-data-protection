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
    public abstract class RabbitMqMessageListenerBase<TModel> : BackgroundService where TModel : class, IMessageListener<TModel>
    {
        private readonly ILogger<RabbitMqMessageListenerBase<TModel>> _logger;
        private readonly RabbitMqConfiguration _configuration;

        protected abstract string Subject { get; }

        private ConnectionFactory _connectionFactory;
        private ConnectionFactory ConnectionFactory
        {
            get
            {
                if (_connectionFactory == null)
                {
                    _connectionFactory = new ConnectionFactory()
                    {
                        HostName = _configuration.Hostname,
                        Port = _configuration.Port,
                        UserName = _configuration.UserName,
                        Password = _configuration.Password
                    };
                }
                
                return _connectionFactory;
            }
        }

        private IConnection Connection => ConnectionFactory.CreateConnection();

        private IModel _channel; 

        protected RabbitMqMessageListenerBase(IOptions<RabbitMqConfiguration> options, ILogger<RabbitMqMessageListenerBase<TModel>> logger)
        {
            _logger = logger;
            _configuration = options.Value;
            
            Init();
        }
        
        protected abstract void HandleMessage(TModel model);
        
        protected sealed override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            EventingBasicConsumer consumer = new EventingBasicConsumer(_channel);

            consumer.Received += HandleMessage;

            _channel.BasicConsume(string.Empty, false, consumer);

            return Task.CompletedTask;
        }

        private void HandleMessage(object sender, BasicDeliverEventArgs args)
        {
            if (ShouldHandleMessage(args))
            {
                TModel model = args.GetModel<TModel>();
             
                _logger.LogInformation("Handling message with subject {GetSubject} and model {Model}", args.GetSubject(), model);

                HandleMessage(model);
                
                _channel.BasicAck(args.DeliveryTag, false);
            }
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
            _channel.ExchangeDeclare(_configuration.Exchange, ExchangeType.Fanout);
            
            QueueDeclareOk result = _channel.QueueDeclare(string.Empty, exclusive: true);

            _channel.QueueBind(result.QueueName, _configuration.Exchange, string.Empty);
        }
        
        private bool ShouldHandleMessage(BasicDeliverEventArgs args)
        {
            return args.GetSubject()?.Equals(Subject, StringComparison.OrdinalIgnoreCase)
                ?? false;
        }
    }
}