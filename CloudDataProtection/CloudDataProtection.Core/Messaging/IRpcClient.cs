using System.Threading.Tasks;

namespace CloudDataProtection.Core.Messaging
{
    public interface IRpcClient<TRequest, TResponse>
    {
        Task<TResponse> Request(TRequest request);
    }
}