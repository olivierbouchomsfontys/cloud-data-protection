using CloudDataProtection.Core.Messaging.Rpc;

namespace CloudDataProtection.Messaging.Server.Dto
{
    public class GetUserEmailOutput : RpcResponse
    {
        public string Email { get; set; }
    }
}