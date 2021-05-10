using System.Threading.Tasks;

namespace CloudDataProtection.Core.Messaging
{
    public interface IRpcClientBase<TRequest, TResponse>
    {
        Task<TResponse> Request(TRequest request);
    }
}