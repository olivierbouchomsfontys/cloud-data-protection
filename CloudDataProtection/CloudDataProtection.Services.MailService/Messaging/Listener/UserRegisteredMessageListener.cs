using System;
using System.Threading.Tasks;
using CloudDataProtection.Services.MailService.Business;
using CloudDataProtection.Services.MailService.Dto;
using CloudDataProtection.Core.Messaging.RabbitMq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CloudDataProtection.Services.MailService.Messaging.Listener
{
    public class UserRegisteredMessageListener : RabbitMqMessageListenerBase<UserRegisteredModel>
    {
        private readonly RegistrationMailLogic _logic;

        public UserRegisteredMessageListener(IOptions<RabbitMqConfiguration> options, ILogger<RabbitMqMessageListenerBase<UserRegisteredModel>> logger, RegistrationMailLogic logic) : base(options, logger)
        {
            _logic = logic;
        }

        protected override string Subject => "UserRegistered";
        public override async Task HandleMessage(UserRegisteredModel model)
        {
            await _logic.SendUserRegistered(model.Email);
        }
    }
}