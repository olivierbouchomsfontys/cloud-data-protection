using CloudDataProtection.Core.Messaging;
using CloudDataProtection.Core.Messaging.RabbitMq;
using CloudDataProtection.Dto;
using Microsoft.Extensions.Options;

namespace CloudDataProtection.Messaging.Publisher
{
    public class UserRegisteredMessagePublisher : RabbitMqMessagePublisherBase<UserResult>
    {
        public UserRegisteredMessagePublisher(IOptions<RabbitMqConfiguration> options) : base(options)
        {
        }

        protected override string RoutingKey => RoutingKeys.UserRegistered;
    }
}