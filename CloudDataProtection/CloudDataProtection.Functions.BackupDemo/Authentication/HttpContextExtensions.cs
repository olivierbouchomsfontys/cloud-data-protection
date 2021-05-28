using CloudDataProtection.Core.Environment;
using Microsoft.AspNetCore.Http;

namespace CloudDataProtection.Functions.BackupDemo.Authentication
{
    public static class HttpContextExtensions
    {
        private static readonly string AuthIndex = "x-functions-key";

        private static readonly string ApiKey = EnvironmentVariableHelper.GetEnvironmentVariable("CDP_DEMO_API_KEY");
        
        public static bool IsAuthenticated(this HttpContext context)
        {
            string token = context.Request.Headers[AuthIndex];

            if (string.IsNullOrWhiteSpace(token))
            {
                return false;
            }

            return ApiKey == token;
        }
    }
}