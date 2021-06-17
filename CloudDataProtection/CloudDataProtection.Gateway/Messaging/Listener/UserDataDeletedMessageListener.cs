using System;
using System.Threading.Tasks;
using CloudDataProtection.Business;
using CloudDataProtection.Core.Messaging;
using CloudDataProtection.Core.Messaging.RabbitMq;
using CloudDataProtection.Core.Result;
using CloudDataProtection.Entities;
using CloudDataProtection.Messaging.Publisher;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CloudDataProtection.Messaging.Listener
{
    public class UserDataDeletedMessageListener : RabbitMqMessageListener<UserDataDeletedModel>
    {
        private readonly IServiceScope _scope;
        
        public UserDataDeletedMessageListener(IOptions<RabbitMqConfiguration> options, ILogger<UserDataDeletedMessageListener> logger, IServiceScopeFactory serviceScopeFactory) : base(options, logger)
        {
            _scope = serviceScopeFactory.CreateScope();
        }

        protected override string RoutingKey => RoutingKeys.UserDataDeleted;
        protected override string Queue => "4E837635-178E-4063-981B-5B9B56C07CAD";
        public override async Task HandleMessage(UserDataDeletedModel model)
        {
            UserBusinessLogic logic = _scope.ServiceProvider.GetRequiredService<UserBusinessLogic>();

            UserDeletionHistoryProgress history = new UserDeletionHistoryProgress
            {
                ServiceName = model.Service,
                StartedAt = model.StartedAt,
                CompletedAt = model.CompletedAt,
            };

            BusinessResult<Tuple<UserDeletionHistory, string>> addProgressResult = await logic.AddProgress(history, model.UserId);

            if (addProgressResult.Success && addProgressResult.Data != null && addProgressResult.Data.Item1.IsComplete)
            {
                UserDeletionHistory progress = addProgressResult.Data.Item1;
                string email = addProgressResult.Data.Item2;
                
                IMessagePublisher<UserDeletionCompleteModel> publisher =
                    _scope.ServiceProvider.GetRequiredService<IMessagePublisher<UserDeletionCompleteModel>>();

                UserDeletionCompleteModel deletionCompleteModel = new UserDeletionCompleteModel
                {
                    UserId = progress.UserId,
                    Email = email
                };

                await publisher.Send(deletionCompleteModel);
            }
        }
    }
    
    public class UserDataDeletedModel
    {
        public long UserId { get; set; }
        
        public DateTime StartedAt { get; set; }
        
        public DateTime CompletedAt { get; set; }

        public string Service { get; set; }
    }
}