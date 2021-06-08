using System.Threading.Tasks;

namespace CloudDataProtection.Core.Messaging
{
    public interface IMessageListener<in TModel>
    {
        Task HandleMessage(TModel model);
    }
}