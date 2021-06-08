using System.Net;

namespace CloudDataProtection.Core.Rest.Errors
{
    public class InternalServerErrorResponse : IErrorResponse
    {
        public string Message { get; }
        public string Title { get; } = "Error";
        public int Status => (int) HttpStatusCode.InternalServerError;
        public string StatusDescription => "Internal server error";

        private InternalServerErrorResponse(string message)
        {
            Message = message;
        }
    }
}