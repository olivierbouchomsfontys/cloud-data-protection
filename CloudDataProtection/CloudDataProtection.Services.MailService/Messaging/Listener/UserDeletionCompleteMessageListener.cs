using System.Threading.Tasks;
using CloudDataProtection.Core.Messaging;
using CloudDataProtection.Core.Messaging.RabbitMq;
using CloudDataProtection.Services.MailService.Business;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CloudDataProtection.Services.MailService.Messaging.Listener
{
    public class UserDeletionCompleteMessageListener : RabbitMqMessageListener<UserDeletionCompleteModel>
    {
        private readonly AccountMailLogic _logic;

        public UserDeletionCompleteMessageListener(IOptions<RabbitMqConfiguration> options, ILogger<UserDeletionCompleteMessageListener> logger, AccountMailLogic logic) : base(options, logger)
        {
            _logic = logic;
        }

        protected override string RoutingKey => RoutingKeys.UserDeletionComplete;
        protected override string Queue => "C5256413-C463-4AC3-92A7-EBFBA6CFA038";
        public override async Task HandleMessage(UserDeletionCompleteModel model)
        {
            await _logic.SendUserDeletionComplete(model.Email);
        }
    }

    public class UserDeletionCompleteModel
    {
        public string Email { get; set; }
    }
}