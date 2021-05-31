using System;
using System.Threading.Tasks;
using CloudDataProtection.Core.Messaging;
using CloudDataProtection.Core.Messaging.RabbitMq;
using CloudDataProtection.Services.Subscription.Business;
using CloudDataProtection.Services.Subscription.Messaging.Dto;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CloudDataProtection.Services.Subscription.Messaging.Listener
{
    public class UserDeletedMessageListener : RabbitMqMessageListener<UserDeletedModel>
    {
        private readonly IServiceScope _scope;

        public UserDeletedMessageListener(IOptions<RabbitMqConfiguration> options, ILogger<UserDeletedMessageListener> logger, IServiceScopeFactory serviceScopeFactory) : base(options, logger)
        {
            _scope = serviceScopeFactory.CreateScope();
        }

        protected override string RoutingKey => RoutingKeys.UserDeleted;
        protected override string Queue => "4E252C81-09AD-4831-A4A7-AFA0073F2392";
        
        public override async Task HandleMessage(UserDeletedModel model)
        {
            DateTime start = DateTime.Now;
            
            BackupConfigurationBusinessLogic logic =
                _scope.ServiceProvider.GetRequiredService<BackupConfigurationBusinessLogic>();
            
            await logic.DeleteByUser(model.UserId);

            IMessagePublisher<UserDataDeletedModel> publisher =
                _scope.ServiceProvider.GetRequiredService<IMessagePublisher<UserDataDeletedModel>>();

            UserDataDeletedModel dataDeletedModel = new UserDataDeletedModel
            {
                UserId = model.UserId,
                StartedAt = start,
                CompletedAt = DateTime.Now
            };

            await publisher.Send(dataDeletedModel);
        }
    }
}