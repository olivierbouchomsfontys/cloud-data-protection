using System.Threading.Tasks;

namespace CloudDataProtection.Core.Messaging
{
    public interface IRpcServerBase<TRequest, TResponse>
    {
        Task<TResponse> HandleMessage(TRequest model);
    }
}