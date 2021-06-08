using CloudDataProtection.Core.Messaging;
using CloudDataProtection.Core.Messaging.RabbitMq;
using CloudDataProtection.Dto;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CloudDataProtection.Messaging.Publisher
{
    public class UserRegisteredMessagePublisher : RabbitMqMessagePublisher<UserResult>
    {
        public UserRegisteredMessagePublisher(IOptions<RabbitMqConfiguration> options, ILogger<UserRegisteredMessagePublisher> logger) : base(options, logger)
        {
        }

        protected override string RoutingKey => RoutingKeys.UserRegistered;
    }
}