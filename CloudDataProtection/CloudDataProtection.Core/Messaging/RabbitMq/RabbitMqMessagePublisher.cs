using System;
using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using RabbitMQ.Client;

namespace CloudDataProtection.Core.Messaging.RabbitMq
{
    public abstract class RabbitMqMessagePublisher<TModel> : IMessagePublisher<TModel> where TModel : class  
    {
        private readonly ILogger<RabbitMqMessagePublisher<TModel>> _logger;
        
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
                }
                
                return _connectionFactory;
            }
        }

        private IConnection _connection;
        private IConnection Connection => _connection ??= ConnectionFactory.CreateConnection();

        private IModel _channel; 

        protected RabbitMqMessagePublisher(IOptions<RabbitMqConfiguration> options, ILogger<RabbitMqMessagePublisher<TModel>> logger)
        {
            _logger = logger;
            _configuration = options.Value;

            Init();
        }
        
        public async Task Send(TModel obj)
        {
            IBasicProperties message = _channel.CreateBasicProperties();

            message.ContentType = _configuration.ContentType;

            byte[] body = JsonSerializer.SerializeToUtf8Bytes(obj, typeof(TModel));
            
            Stopwatch stopwatch = Stopwatch.StartNew();

            await DoSend(message, body);

            _connection?.Close();

            stopwatch.Stop();

            _logger.Log(LogLevel.Information, "{Type}.{Method} to {Host} took {Ms}ms", this.GetType().Name, nameof(Send), _configuration.Hostname,
                stopwatch.ElapsedMilliseconds);
        }

        private async Task DoSend(IBasicProperties message, byte[] body)
        {
            await Task.Run(() =>
            {
                _channel.BasicPublish(_configuration.Exchange, RoutingKey, message, body);
            });
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
            _logger.LogWarning("Retrying initialization. Waiting {Ms}ms. Attempt nr. {Attempt} / {RetryCount}", timeSpan.TotalMilliseconds, attempt, _configuration.RetryCount);
        }
        
        private TimeSpan GetRetryDelay(int attempt)
        {
            return TimeSpan.FromMilliseconds(attempt * _configuration.RetryDelayMs);
        }

        private void DoInit()
        {
            _channel = Connection.CreateModel();
            _channel.ExchangeDeclare(_configuration.Exchange, ExchangeType.Fanout, true);
        }
    }
}