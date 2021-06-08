using System;
using System.Net;
using CloudDataProtection.Core.Rest.Errors.Action;

namespace CloudDataProtection.Core.Rest.Errors
{
    public class NotFoundResponse : IErrorResponse
    {
        public string Title { get; } = "Error";
        public string Message { get; }

        public int Status => (int) HttpStatusCode.NotFound;
        public string StatusDescription => "Not found";

        public static NotFoundResponse Create(string type, object id)
        {
            string message = $"Could not find {type.ToLower()} with id = {id}";
            
            return new NotFoundResponse(message);
        }
        
        private static NotFoundResponse Create(Type type, object id, CrudAction action)
        {
            string message = $"Could not {action.ToString().ToLower()} {type.Name.ToLower()} with id = {id}";
            
            return new NotFoundResponse(message);
        }

        private NotFoundResponse(string message)
        {
            Message = message;
        }
    }
}