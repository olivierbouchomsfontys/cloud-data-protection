namespace CloudDataProtection.Core.Messaging
{
    public interface IMessageListener<TModel>
    {
        void HandleMessage(TModel model);
    }
}