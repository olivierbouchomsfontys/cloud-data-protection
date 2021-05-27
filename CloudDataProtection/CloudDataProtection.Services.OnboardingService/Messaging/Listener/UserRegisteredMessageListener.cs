using System.Threading.Tasks;
using CloudDataProtection.Core.Messaging;
using CloudDataProtection.Core.Messaging.RabbitMq;
using CloudDataProtection.Services.Onboarding.Business;
using CloudDataProtection.Services.Onboarding.Dto;
using CloudDataProtection.Services.Onboarding.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CloudDataProtection.Services.Onboarding.Messaging.Listener
{
    public class UserRegisteredMessageListener : RabbitMqMessageListener<UserRegisteredModel>
    {
        private readonly OnboardingBusinessLogic _logic;

        public UserRegisteredMessageListener(IOptions<RabbitMqConfiguration> options, ILogger<UserRegisteredMessageListener> logger, OnboardingBusinessLogic logic) : base(options, logger)
        {
            _logic = logic;
        }

        protected override string RoutingKey => RoutingKeys.UserRegistered;
        protected override string Queue => "42D7C890-F91D-4343-8D8D-0CA0F11AF793";
        public override async Task HandleMessage(UserRegisteredModel model)
        {
            if (model.Role == UserRegisteredRole.Client)
            {
                Entities.Onboarding onboarding = new Entities.Onboarding
                {
                    Status = OnboardingStatus.None,
                    UserId = model.Id
                };

                await _logic.Create(onboarding);
            }
        }
    }
}