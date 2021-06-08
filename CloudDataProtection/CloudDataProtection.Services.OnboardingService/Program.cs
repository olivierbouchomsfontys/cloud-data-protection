using CloudDataProtection.Core.DependencyInjection.Extensions;
using CloudDataProtection.Core.Papertrail.Extensions;
using CloudDataProtection.Services.Onboarding.Data.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace CloudDataProtection.Services.Onboarding
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args)
                .Build()
                .Migrate<IOnboardingDbContext, OnboardingDbContext>()
                .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureLogging(loggingBuilder => loggingBuilder.ConfigureLogging())
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        }
    }
}