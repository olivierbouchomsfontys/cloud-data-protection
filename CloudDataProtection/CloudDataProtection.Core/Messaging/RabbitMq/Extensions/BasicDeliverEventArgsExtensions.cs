using System.Linq;
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

        public static string GetSubject(this BasicDeliverEventArgs args)
        {
            return args.BasicProperties.Headers?
                .Where(c => c.Key == "Subject")
                .Select(t => ParseHeaderString(t.Value))
                .FirstOrDefault();
        }

        private static string ParseHeaderString(object obj)
        {
            if (obj is byte[] bytes)
            {
                return Encoding.UTF8.GetString(bytes);
            }

            return null;
        }
    }
}