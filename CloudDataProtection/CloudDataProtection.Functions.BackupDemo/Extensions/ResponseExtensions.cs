using Azure;

namespace CloudDataProtection.Functions.BackupDemo.Extensions
{
    public static class ResponseExtensions
    {
        public static bool IsSuccessStatusCode<T>(this Response<T> response)
        {
            return response.GetRawResponse().Status.ToString().StartsWith("2");
        }
    }
}