using System.Threading.Tasks;

namespace CloudDataProtection.Core.Messaging
{
    public interface IRpcClient<in TRequest, TResponse>
    {
        Task<TResponse> Request(TRequest request);
    }
}