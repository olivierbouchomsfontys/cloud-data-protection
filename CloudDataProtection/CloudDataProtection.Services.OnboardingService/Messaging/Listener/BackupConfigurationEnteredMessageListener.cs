using System;
using System.Threading.Tasks;
using CloudDataProtection.Core.Messaging;
using CloudDataProtection.Core.Messaging.RabbitMq;
using CloudDataProtection.Core.Result;
using CloudDataProtection.Services.Onboarding.Business;
using CloudDataProtection.Services.Onboarding.Dto;
using CloudDataProtection.Services.Onboarding.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CloudDataProtection.Services.Onboarding.Messaging.Listener
{
    public class BackupConfigurationEnteredMessageListener : RabbitMqMessageListenerBase<BackupConfigurationEnteredModel>
    {
        private readonly IServiceScope _scope;

        public BackupConfigurationEnteredMessageListener(IOptions<RabbitMqConfiguration> options, ILogger<BackupConfigurationEnteredMessageListener> logger, IServiceProvider serviceProvider) : base(options, logger)
        {
            _scope = serviceProvider.CreateScope();
        }

        protected override string RoutingKey => RoutingKeys.BackupConfigurationEntered;
        protected override string Queue => "AC3585E6-23C1-47B0-9904-99F2C5965721";
        public override async Task HandleMessage(BackupConfigurationEnteredModel model)
        {
            OnboardingBusinessLogic logic = _scope.ServiceProvider.GetRequiredService<OnboardingBusinessLogic>();
            
            BusinessResult<Entities.Onboarding> result = await logic.GetByUser(model.UserId);

            if (result.Success)
            {
                result.Data.Status = OnboardingStatus.SchemeEntered;

                await logic.Update(result.Data);
            }
        }
    }
}