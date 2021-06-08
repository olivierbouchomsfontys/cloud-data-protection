using System.Threading.Tasks;

namespace CloudDataProtection.Core.Messaging
{
    public interface IRpcServer<in TRequest, TResponse>
    {
        Task<TResponse> HandleMessage(TRequest model);
    }
}