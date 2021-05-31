using CloudDataProtection.Core.Messaging;
using CloudDataProtection.Core.Messaging.RabbitMq;
using CloudDataProtection.Services.Subscription.Messaging.Dto;
using Microsoft.Extensions.Options;

namespace CloudDataProtection.Services.Subscription.Messaging.Publisher
{
    public class UserDataDeletedMessagePublisher : RabbitMqMessagePublisher<UserDataDeletedModel>
    {
        public UserDataDeletedMessagePublisher(IOptions<RabbitMqConfiguration> options) : base(options)
        {
        }

        protected override string RoutingKey => RoutingKeys.UserDataDeleted;
    }
}