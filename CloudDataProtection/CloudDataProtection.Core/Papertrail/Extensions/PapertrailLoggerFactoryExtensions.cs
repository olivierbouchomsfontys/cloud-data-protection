using System;
using CloudDataProtection.Core.Environment;
using CloudDataProtection.Core.Papertrail.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;

namespace CloudDataProtection.Core.Papertrail.Extensions
{
    public static class PapertrailLoggerFactoryExtensions
    {
        private const string UrlKey = "CDP_PAPERTRAIL_URL";
        private const string TokenKey = "CDP_PAPERTRAIL_ACCESS_TOKEN";
        
        public static ILoggingBuilder AddPapertrail(this ILoggingBuilder builder)
        {
            builder.AddConfiguration();

            string url = EnvironmentVariableHelper.GetEnvironmentVariable(UrlKey);
            string accessToken = EnvironmentVariableHelper.GetEnvironmentVariable(TokenKey);

            if (string.IsNullOrEmpty(url))
            {
                Console.WriteLine($"Could not find environment variable {UrlKey}. Skipping adding Papertrail.");
                return builder;
            }

            if (string.IsNullOrEmpty(accessToken))
            {
                Console.WriteLine($"Could not find environment variable {TokenKey}. Skipping adding Papertrail.");
                return builder;
            }

            builder.Services.Configure<PapertrailOptions>(options =>
            {
                options.Url = url;
                options.AccessToken = accessToken;
            });
            
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, PapertrailLoggerProvider>());
            
            LoggerProviderOptions.RegisterProviderOptions<PapertrailOptions, PapertrailLoggerProvider>(builder.Services);
            
            return builder;
        } 
    }
}