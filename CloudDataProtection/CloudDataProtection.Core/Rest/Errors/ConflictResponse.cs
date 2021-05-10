using System.Net;

namespace CloudDataProtection.Core.Rest.Errors
{
    public class ConflictResponse : IErrorResponse
    {
        public string Message { get; }
        public string Title { get; } = "Error";
        public int Status => (int) HttpStatusCode.Conflict;
        public string StatusDescription => "Conflict";

        public static ConflictResponse Create(string message)
        {
            return new ConflictResponse(message);
        }

        private ConflictResponse(string message)
        {
            Message = message;
        }
    }
}