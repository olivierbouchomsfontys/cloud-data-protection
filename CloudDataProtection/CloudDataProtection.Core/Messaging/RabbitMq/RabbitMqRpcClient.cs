using System;
using System.Threading.Tasks;
using CloudDataProtection.Core.Messaging.RabbitMq.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace CloudDataProtection.Core.Messaging.RabbitMq
{
    /// <summary>
    /// Single use RPC Client for RabbitMq
    /// </summary>
    public abstract class RabbitMqRpcClient<TRequest, TResponse> : IRpcClient<TRequest, TResponse>
    {
        private readonly ILogger<RabbitMqRpcClient<TRequest, TResponse>> _logger;
        private readonly RabbitMqConfiguration _configuration;

        private static readonly string Queue = "rpc_queue";
        
        private string _correlationId;
        
        private IBasicProperties _properties;

        private IModel _requestChannel;
        private IModel _replyChannel;
        
        private TResponse _response;

        private bool _hasError;

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


        public RabbitMqRpcClient(IOptions<RabbitMqConfiguration> options, ILogger<RabbitMqRpcClient<TRequest, TResponse>> logger)
        {
            _configuration = options.Value;
            _logger = logger;
        }

        public async Task<TResponse> Request(TRequest request)
        {
            Init();

            byte[] body = JsonSerializer.SerializeToUtf8Bytes(request, typeof(TRequest));

            try
            {
                return await DoRequest(_properties, body);
            } 
            finally
            {
                _connection?.Close();
                _connection?.Dispose();
                
                _requestChannel?.Close();
                _requestChannel?.Dispose();

                _replyChannel?.Close();
                _replyChannel?.Dispose();
            }
        }

        private void Init()
        {
            _requestChannel = Connection.CreateModel();
            _replyChannel = Connection.CreateModel();
            
            _requestChannel.ExchangeDeclare(_configuration.Exchange, ExchangeType.Fanout, true);
            
            // Create and bind RPC queue
            _requestChannel.QueueDeclare(Queue, exclusive: false, durable: false, autoDelete: false);
            _requestChannel.QueueBind(Queue, _configuration.Exchange, "");
            
            // Create and bind RPC reply queue
            string replyQueueName = $"rpc_reply_queue_{Guid.NewGuid().ToString()}";
                
            _replyChannel.QueueDeclare(replyQueueName, false, false, false);
            _replyChannel.QueueBind(replyQueueName, _configuration.Exchange, replyQueueName);

            EventingBasicConsumer consumer = new EventingBasicConsumer(_replyChannel);
            
            consumer.Received += (_ ,args) => OnResponseReceived(args);

            _replyChannel.BasicConsume(replyQueueName, true, consumer);

            _properties = _requestChannel.CreateBasicProperties();
            
            _properties.ReplyTo = replyQueueName;
            _properties.ContentType = _configuration.ContentType;
            _properties.Persistent = true;
        }
                
        private async Task<TResponse> DoRequest(IBasicProperties message, byte[] body)
        {
            string correlationId = Guid.NewGuid().ToString();
            
            message.ClearCorrelationId();
            message.CorrelationId = correlationId;
            
            _correlationId = message.CorrelationId;
            
            _requestChannel.BasicPublish(_configuration.Exchange, "", message, body);
            
            while (_response == null&& !_hasError)
            {
                await Task.Delay(8);
            }

            return _response;
        }

        private void OnResponseReceived(BasicDeliverEventArgs args)
        {
            if (ShouldHandleResponse(args))
            {
                try
                {
                    _response = args.GetModel<TResponse>();
                }
                catch (JsonReaderException e)
                {
                    _logger.LogWarning(e, "Could not deserialize received object. Length: {Length}", args.Body.Length);

                    _replyChannel.BasicReject(args.DeliveryTag, false);
                    _hasError = true;
                    return;
                }
                
                _replyChannel.BasicAck(args.DeliveryTag, false);
            }
        }

        private bool ShouldHandleResponse(BasicDeliverEventArgs args)
        {
            if (args.BasicProperties.IsReplyToPresent())
            {
                return false;
            }
            
            return args.BasicProperties.IsCorrelationIdPresent() && 
                   args.BasicProperties.CorrelationId == _correlationId;
        }
    }
}