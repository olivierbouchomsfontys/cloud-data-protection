using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;

namespace CloudDataProtection.Core.Messaging.RabbitMq.Extensions
{
    public static class BasicDeliverEventArgsExtensions
    {
        public static T GetModel<T>(this BasicDeliverEventArgs args)
        {
            string content = Encoding.UTF8.GetString(args.Body.ToArray());
            
            return JsonConvert.DeserializeObject<T>(content);
        }
    }
}