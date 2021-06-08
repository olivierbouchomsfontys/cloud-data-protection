using System.Threading.Tasks;

namespace CloudDataProtection.Core.Messaging
{
    public interface IMessagePublisher<in TModel>
    {
        Task Send(TModel model);
    }
}