using System.Threading.Tasks;
using CloudDataProtection.Core.Messaging;
using CloudDataProtection.Core.Messaging.RabbitMq;
using CloudDataProtection.Services.MailService.Business;
using CloudDataProtection.Services.MailService.Dto;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CloudDataProtection.Services.MailService.Messaging.Listener
{
    public class GoogleAccountConnectedMessageListener : RabbitMqMessageListener<GoogleAccountConnectedModel>
    {
        private readonly AccountMailLogic _logic;

        public GoogleAccountConnectedMessageListener(IOptions<RabbitMqConfiguration> options, ILogger<GoogleAccountConnectedMessageListener> logger, AccountMailLogic logic) : base(options, logger)
        {
            _logic = logic;
        }

        protected override string RoutingKey => RoutingKeys.GoogleAccountConnected;
        protected override string Queue => "2D0A7AE4-0AF4-468E-B775-54DB46980AD9";
        
        public override async Task HandleMessage(GoogleAccountConnectedModel model)
        {
            await _logic.SendAccountConnected(model.Email);
        }
    }
}