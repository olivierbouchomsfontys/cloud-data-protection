using System.Collections.Generic;
using RabbitMQ.Client;

namespace CloudDataProtection.Core.Messaging.RabbitMq.Extensions
{
    public static class BasicPropertiesExtensions
    {
        public static void SetSubject(this IBasicProperties message, string subject)
        {
            message.Headers ??= new Dictionary<string, object>();
            
            message.Headers.Add("Subject", subject);
        }
    }
}