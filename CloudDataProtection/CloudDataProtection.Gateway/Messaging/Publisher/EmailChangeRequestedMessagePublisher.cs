using System;
using CloudDataProtection.Core.Messaging;
using CloudDataProtection.Core.Messaging.RabbitMq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CloudDataProtection.Messaging.Publisher
{
    public class EmailChangeRequestedMessagePublisher : RabbitMqMessagePublisher<EmailChangeRequestedModel>
    {
        public EmailChangeRequestedMessagePublisher(IOptions<RabbitMqConfiguration> options, ILogger<EmailChangeRequestedMessagePublisher> logger) : base(options, logger)
        {
        }

        protected override string RoutingKey => RoutingKeys.EmailChangeRequested;
    }

    public class EmailChangeRequestedModel
    {
        public string NewEmail { get; set; }
        
        public string Url { get; set; }
        
        public DateTime ExpiresAt { get; set; }
    }
}