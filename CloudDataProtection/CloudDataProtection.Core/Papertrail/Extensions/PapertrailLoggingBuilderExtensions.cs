using CloudDataProtection.Core.Environment;
using Microsoft.Extensions.Logging;

namespace CloudDataProtection.Core.Papertrail.Extensions
{
    public static class PapertrailLoggingBuilderExtensions
    {
        public static void ConfigureLogging(this ILoggingBuilder builder)
        {
            if (EnvironmentVariableHelper.GetHostingEnvironment() == Environment.Environment.Development)
            {
                builder = builder.AddConsole();
            }
            else
            {
                builder = builder.ClearProviders();
                builder = builder.AddPapertrail();
            }

            builder
                .AddFilter("Microsoft.EntityFrameworkCore.Migrations", LogLevel.None);
        }
    }
}