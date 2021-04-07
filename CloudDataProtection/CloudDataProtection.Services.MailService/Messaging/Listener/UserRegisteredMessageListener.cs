using System;
using ClouDataProtection.Services.MailService.Dto;
using CloudDataProtection.Core.Messaging.RabbitMq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ClouDataProtection.Services.MailService.Messaging.Listener
{
    public class UserRegisteredMessageListener : RabbitMqMessageListenerBase<UserRegisteredModel>
    {
        public UserRegisteredMessageListener(IOptions<RabbitMqConfiguration> options, ILogger<RabbitMqMessageListenerBase<UserRegisteredModel>> logger) : base(options, logger)
        {
        }

        protected override string Subject => "UserRegistered";
        public override void HandleMessage(UserRegisteredModel model)
        {
            // TODO Handle
            Console.WriteLine(model);
        }
    }
}