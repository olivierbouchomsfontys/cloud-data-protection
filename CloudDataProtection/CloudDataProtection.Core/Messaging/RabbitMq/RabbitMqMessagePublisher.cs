﻿using System.Runtime.InteropServices.ComTypes;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace CloudDataProtection.Core.Messaging.RabbitMq
{
    public abstract class RabbitMqMessagePublisher<TModel> : IMessagePublisher<TModel> where TModel : class  
    {
        private readonly RabbitMqConfiguration _configuration;

        protected abstract string RoutingKey { get; }

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
                        VirtualHost = _configuration.VirtualHost,
                    };
                };
                
                return _connectionFactory;
            }
        }

        private IConnection _connection;
        private IConnection Connection
        {
            get
            {
                if (_connection == null)
                {
                    _connection = ConnectionFactory.CreateConnection();
                }

                return _connection;
            }
        }
        
        private IModel Channel { get; } 

        protected RabbitMqMessagePublisher(IOptions<RabbitMqConfiguration> options)
        {
            _configuration = options.Value;
            
            Channel = Connection.CreateModel();
            Channel.ExchangeDeclare(_configuration.Exchange, ExchangeType.Fanout, true);
        }
        
        public async Task Send(TModel obj)
        {
            IBasicProperties message = Channel.CreateBasicProperties();

            message.ContentType = _configuration.ContentType;

            byte[] body = JsonSerializer.SerializeToUtf8Bytes(obj, typeof(TModel));

            await DoSend(message, body);
            
            _connection?.Close();
        }

        private async Task DoSend(IBasicProperties message, byte[] body)
        {
            await Task.Run(() =>
            {
                Channel.BasicPublish(_configuration.Exchange, RoutingKey, message, body);
            });
        }
    }
}