using CloudDataProtection.Core.Messaging;
using CloudDataProtection.Core.Messaging.RabbitMq;
using CloudDataProtection.Dto;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CloudDataProtection.Messaging.Publisher
{
    public class UserDeletedMessagePublisher : RabbitMqMessagePublisher<UserDeletedModel>
    {
        public UserDeletedMessagePublisher(IOptions<RabbitMqConfiguration> options, ILogger<UserDeletedMessagePublisher> logger) : base(options, logger)
        {
        }

        protected override string RoutingKey => RoutingKeys.UserDeleted;
    }
}