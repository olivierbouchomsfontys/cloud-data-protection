using System;
using System.Threading.Tasks;
using CloudDataProtection.Core.Messaging.RabbitMq;
using CloudDataProtection.Messaging.Server.Dto;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CloudDataProtection.Messaging.Server
{
    public class GetUserEmailRpcServer : RabbitMqRpcServer<GetUserEmailInput, GetUserEmailOutput>
    {
        public GetUserEmailRpcServer(IOptions<RabbitMqConfiguration> options, ILogger<GetUserEmailRpcServer> logger) : base(options, logger)
        {
        }

        public override async Task<GetUserEmailOutput> HandleMessage(GetUserEmailInput model)
        {
            return new GetUserEmailOutput
            {
                Email = $"{model.UserId}@live.nl" + $"{DateTime.Now.ToString()}"
            };
        }
    }
}