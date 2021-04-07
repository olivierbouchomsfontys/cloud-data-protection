using System.Threading.Tasks;

namespace CloudDataProtection.Core.Messaging
{
    public interface IMessagePublisher<TModel>
    {
        Task Send(TModel model);
    }
}