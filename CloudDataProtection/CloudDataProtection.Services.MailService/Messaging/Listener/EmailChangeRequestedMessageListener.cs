using System.Threading.Tasks;
using CloudDataProtection.Core.Messaging;
using CloudDataProtection.Core.Messaging.RabbitMq;
using CloudDataProtection.Services.MailService.Business;
using CloudDataProtection.Services.MailService.Dto;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CloudDataProtection.Services.MailService.Messaging.Listener
{
    public class EmailChangeRequestedMessageListener : RabbitMqMessageListener<EmailChangeRequestedModel>
    {
        private readonly AccountMailLogic _logic;

        public EmailChangeRequestedMessageListener(IOptions<RabbitMqConfiguration> options, ILogger<EmailChangeRequestedMessageListener> logger, AccountMailLogic logic) : base(options, logger)
        {
            _logic = logic;
        }

        protected override string RoutingKey => RoutingKeys.EmailChangeRequested;
        protected override string Queue => "48B17AA1-62AE-4950-B408-91232DCE2E90";
        
        public override async Task HandleMessage(EmailChangeRequestedModel model)
        {
            await _logic.SendEmailChangeRequested(model.NewEmail, model.Url, model.ExpiresAt);
        }
    }
}