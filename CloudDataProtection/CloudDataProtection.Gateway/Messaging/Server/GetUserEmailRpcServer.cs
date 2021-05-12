using System;
using System.Threading.Tasks;
using CloudDataProtection.Business;
using CloudDataProtection.Core.Messaging.RabbitMq;
using CloudDataProtection.Core.Messaging.Rpc;
using CloudDataProtection.Core.Result;
using CloudDataProtection.Entities;
using CloudDataProtection.Messaging.Server.Dto;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CloudDataProtection.Messaging.Server
{
    public class GetUserEmailRpcServer : RabbitMqRpcServer<GetUserEmailInput, GetUserEmailOutput>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public GetUserEmailRpcServer(IOptions<RabbitMqConfiguration> options, ILogger<GetUserEmailRpcServer> logger, IServiceScopeFactory serviceScopeFactory) : base(options, logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public override async Task<GetUserEmailOutput> HandleMessage(GetUserEmailInput model)
        {
            using (IServiceScope scope = _serviceScopeFactory.CreateScope())
            {
                UserBusinessLogic logic = scope.ServiceProvider.GetRequiredService<UserBusinessLogic>();
                
                BusinessResult<User> result = await logic.Get(model.UserId);

                if (!result.Success)
                {
                    return new GetUserEmailOutput
                    {
                        Status = RpcResponseStatus.Error,
                        StatusMessage = result.Message
                    };
                }
        
                return new GetUserEmailOutput
                {
                    Email = result.Data.Email,
                    Status = RpcResponseStatus.Ok
                };
            }
        }
    }
}