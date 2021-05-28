using System;
using System.Threading;
using System.Threading.Tasks;
using CloudDataProtection.Core.Messaging.RabbitMq.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace CloudDataProtection.Core.Messaging.RabbitMq
{
    public abstract class RabbitMqRpcServer<TRequest, TResponse> : BackgroundService, IRpcServer<TRequest, TResponse>
    {
        private readonly ILogger<RabbitMqRpcServer<TRequest, TResponse>> _logger;
        private readonly RabbitMqConfiguration _configuration;

        private static readonly string QueueName = "rpc_queue";
        
        private IModel _channel;

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
        private IConnection Connection
        {
            get
            {
                if (_connection == null || !_connection.IsOpen)
                {
                    _connection = ConnectionFactory.CreateConnection();
                }
                
                return _connection;
            }
        }
        
        public RabbitMqRpcServer(IOptions<RabbitMqConfiguration> options, ILogger<RabbitMqRpcServer<TRequest, TResponse>> logger)
        {
            _logger = logger;
            _configuration = options.Value;

            Init();
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

            _channel.QueueDeclare(QueueName, false, false, false);
            _channel.QueueBind(QueueName, _configuration.Exchange, "");
            
            _channel.BasicQos(0, 1, false);
        }
        
        public abstract Task<TResponse> HandleMessage(TRequest model);

        protected override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            EventingBasicConsumer consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (_, args) => await OnRequestReceive(args);

            _channel.BasicConsume(QueueName, false, consumer);

            return Task.CompletedTask;
        }

        private async Task OnRequestReceive(BasicDeliverEventArgs args)
        {
            TRequest request;

            if (!args.BasicProperties.IsReplyToPresent())
            {
                _channel.BasicReject(args.DeliveryTag, false);
                return;
            }
            
            try
            {
                request = args.GetModel<TRequest>();
            }
            catch (JsonReaderException e)
            {
                _logger.LogWarning("Could not deserialize received object. Length: {Length}", args.Body.Length);
                
                _channel.BasicReject(args.DeliveryTag, false);
                return;
            }

            IBasicProperties replyProperties = _channel.CreateBasicProperties();

            replyProperties.CorrelationId = args.BasicProperties.CorrelationId;
            replyProperties.ContentType = _configuration.ContentType;
            replyProperties.Persistent = true;
            replyProperties.ClearReplyTo();

            TResponse response = await HandleMessage(request);
            
            byte[] body = JsonSerializer.SerializeToUtf8Bytes(response, typeof(TResponse));

            DoSendResponse(replyProperties, body, args.BasicProperties.ReplyTo);
            
            _channel.BasicAck(args.DeliveryTag, false);
        }

        private void DoSendResponse(IBasicProperties message, byte[] body, string routingKey)
        {
            _channel.BasicPublish(_configuration.Exchange, routingKey, message, body);
        }
    }
}