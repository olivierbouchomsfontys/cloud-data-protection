namespace CloudDataProtection.Core.Messaging.Rpc
{
    public class RpcResponse
    {
        public RpcResponseStatus Status { get; set; }
        
        public string StatusMessage { get; set; }
    }
}