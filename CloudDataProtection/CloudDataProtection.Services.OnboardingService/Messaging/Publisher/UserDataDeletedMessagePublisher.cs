using CloudDataProtection.Core.Messaging;
using CloudDataProtection.Core.Messaging.RabbitMq;
using CloudDataProtection.Services.Onboarding.Messaging.Publisher.Dto;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CloudDataProtection.Services.Onboarding.Messaging.Publisher
{
    public class UserDataDeletedMessagePublisher : RabbitMqMessagePublisher<UserDataDeletedModel>
    {
        public UserDataDeletedMessagePublisher(IOptions<RabbitMqConfiguration> options, ILogger<UserDataDeletedMessagePublisher> logger) : base(options, logger)
        {
        }

        protected override string RoutingKey => RoutingKeys.UserDataDeleted;
    }
}