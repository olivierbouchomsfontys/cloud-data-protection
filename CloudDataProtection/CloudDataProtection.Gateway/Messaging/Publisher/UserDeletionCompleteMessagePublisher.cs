using CloudDataProtection.Core.Messaging;
using CloudDataProtection.Core.Messaging.RabbitMq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CloudDataProtection.Messaging.Publisher
{
    public class UserDeletionCompleteMessagePublisher : RabbitMqMessagePublisher<UserDeletionCompleteModel>
    {
        public UserDeletionCompleteMessagePublisher(IOptions<RabbitMqConfiguration> options, ILogger<UserDeletionCompleteMessagePublisher> logger) : base(options, logger)
        {
        }

        protected override string RoutingKey => RoutingKeys.UserDeletionComplete;
    }

    public class UserDeletionCompleteModel
    {
        public long UserId { get; set; }
        
        public string Email { get; set; }
    }
}