using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using CloudDataProtection.Core.Messaging.RabbitMq.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CloudDataProtection.Core.Messaging.RabbitMq
{
    public abstract class RabbitMqRpcClientBase<TRequest, TResponse> : IRpcClientBase<TRequest, TResponse>
    {
        private readonly ILogger<RabbitMqRpcClientBase<TRequest, TResponse>> _logger;
        private readonly RabbitMqConfiguration _configuration;

        private readonly IBasicProperties _properties;

        private const string Queue = "rpc_queue";

        private readonly IDictionary<string, TResponse> _responses = new Dictionary<string, TResponse>();
        private readonly IList<string> _correlationIds = new List<string>();

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
                        Password = _configuration.Password
                    };
                }

                return _connectionFactory;
            }
        }
        
        private IConnection Connection => ConnectionFactory.CreateConnection();

        private readonly IModel _requestChannel;
        private readonly IModel _replyChannel;

        public RabbitMqRpcClientBase(IOptions<RabbitMqConfiguration> options, ILogger<RabbitMqRpcClientBase<TRequest, TResponse>> logger)
        {
            _logger = logger;
            _configuration = options.Value;

            _requestChannel = Connection.CreateModel();
            _replyChannel = Connection.CreateModel();
            
            _requestChannel.ExchangeDeclare(_configuration.Exchange, ExchangeType.Fanout, true);
            
            // Create and bind RPC queue
            _requestChannel.QueueDeclare(Queue, exclusive: false, durable: false, autoDelete: false);
            _requestChannel.QueueBind(Queue, _configuration.Exchange, "");
            
            // Create and bind RPC reply queue
            string replyQueueName = $"rpc_reply_queue{DateTime.Now:s}";
                
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

        public async Task<TResponse> Request(TRequest request)
        {
            byte[] body = JsonSerializer.SerializeToUtf8Bytes(request, typeof(TRequest));

            return await DoRequest(_properties, body);
        }
                
        private async Task<TResponse> DoRequest(IBasicProperties message, byte[] body)
        {
            string correlationId = Guid.NewGuid().ToString();
            
            message.ClearCorrelationId();
            message.CorrelationId = correlationId;
            
            _correlationIds.Add(message.CorrelationId);
            
            _logger.LogInformation("Message sent to {Exchange}", _configuration.Exchange);
            
            _requestChannel.BasicPublish(_configuration.Exchange, "", message, body);
            
            while (!_responses.ContainsKey(correlationId))
            {
                await Task.Delay(100);
            }
            
            return _responses[correlationId];
        }

        private void OnResponseReceived(BasicDeliverEventArgs args)
        {
            string correlationId = args.BasicProperties.CorrelationId;

            if (args.BasicProperties.IsReplyToPresent())
            {
                return;
            }
            
            if (args.BasicProperties.IsCorrelationIdPresent() && _correlationIds.Contains(correlationId))
            {
                TResponse model = args.GetModel<TResponse>();
                
                _logger.LogInformation("Handling message with correlation id {CorrelationId} subject {GetSubject} and model {Model}", args.BasicProperties.CorrelationId, args.RoutingKey, model);
                
                _responses.Add(correlationId, model);

                _correlationIds.Remove(args.BasicProperties.CorrelationId);

                _replyChannel.BasicAck(args.DeliveryTag, false);
            }
            else
            {
                _replyChannel.BasicReject(args.DeliveryTag, false);
            }
        }
    }
}