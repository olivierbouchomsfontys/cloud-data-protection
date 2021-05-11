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
    /// <summary>
    /// Single use RPC Client for RabbitMq
    /// </summary>
    public abstract class RabbitMqRpcClientBase<TRequest, TResponse> : IRpcClientBase<TRequest, TResponse>
    {
        private readonly RabbitMqConfiguration _configuration;

        private const string Queue = "rpc_queue";
        
        private bool _isUsed = false;

        private TResponse _response;
        
        private string _correlationId;

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

        private IBasicProperties _properties;

        private IModel _requestChannel;
        private IModel _replyChannel;

        public RabbitMqRpcClientBase(IOptions<RabbitMqConfiguration> options)
        {
            _configuration = options.Value;
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
            if (_isUsed)
            {
                throw new InvalidOperationException("Client has already been used!");
            }
            
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

            _isUsed = true;
        }
                
        private async Task<TResponse> DoRequest(IBasicProperties message, byte[] body)
        {
            string correlationId = Guid.NewGuid().ToString();
            
            message.ClearCorrelationId();
            message.CorrelationId = correlationId;
            
            _correlationId = message.CorrelationId;
            
            _requestChannel.BasicPublish(_configuration.Exchange, "", message, body);
            
            while (_response == null)
            {
                await Task.Delay(8);
            }

            return _response;
        }

        private void OnResponseReceived(BasicDeliverEventArgs args)
        {
            if (ShouldHandleResponse(args))
            {
                _response = args.GetModel<TResponse>();
                
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