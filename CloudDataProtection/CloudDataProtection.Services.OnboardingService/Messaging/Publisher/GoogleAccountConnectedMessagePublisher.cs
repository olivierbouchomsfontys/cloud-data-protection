using CloudDataProtection.Core.Messaging;
using CloudDataProtection.Core.Messaging.RabbitMq;
using CloudDataProtection.Services.Onboarding.Messaging.Publisher.Dto;
using Microsoft.Extensions.Options;

namespace CloudDataProtection.Services.Onboarding.Messaging.Publisher
{
    public class GoogleAccountConnectedMessagePublisher : RabbitMqMessagePublisherBase<GoogleAccountConnectedModel>
    {
        public GoogleAccountConnectedMessagePublisher(IOptions<RabbitMqConfiguration> options) : base(options)
        {
        }

        protected override string RoutingKey => RoutingKeys.GoogleAccountConnected;
    }
}