using System.Threading.Tasks;

namespace CloudDataProtection.Core.Messaging
{
    public interface IMessageListener<TModel>
    {
        Task HandleMessage(TModel model);
    }
}