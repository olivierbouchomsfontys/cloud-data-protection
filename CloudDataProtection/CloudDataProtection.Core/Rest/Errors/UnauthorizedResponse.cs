using System.Net;

namespace CloudDataProtection.Core.Rest.Errors
{
    public class UnauthorizedResponse : IErrorResponse
    {
        public string Title => "Error";
        public string Message { get; }

        public int Status => (int) HttpStatusCode.Unauthorized;
        public string StatusDescription => "Unauthorized";

        public static UnauthorizedResponse Create()
        {
            string message = "Invalid username or password";
            
            return new UnauthorizedResponse(message);
        }
        
        private UnauthorizedResponse(string message)
        {
            Message = message;
        }
    }
}