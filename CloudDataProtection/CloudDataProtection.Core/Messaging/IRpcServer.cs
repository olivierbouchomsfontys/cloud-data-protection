using System.Threading.Tasks;

namespace CloudDataProtection.Core.Messaging
{
    public interface IRpcServer<TRequest, TResponse>
    {
        Task<TResponse> HandleMessage(TRequest model);
    }
}