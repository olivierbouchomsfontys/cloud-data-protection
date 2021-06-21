using CloudDataProtection.Core.Messaging.RabbitMq;
using CloudDataProtection.Services.Onboarding.Messaging.Client.Dto;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CloudDataProtection.Services.Onboarding.Messaging.Client
{
    public class GetUserEmailRpcClient : RabbitMqRpcClient<GetUserEmailInput, GetUserEmailOutput>
    {
        public GetUserEmailRpcClient(IOptions<RabbitMqConfiguration> options, ILogger<GetUserEmailRpcClient> logger) : base(options, logger)
        {
        }
    }
}