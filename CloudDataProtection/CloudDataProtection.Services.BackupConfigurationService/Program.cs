using System.Threading.Tasks;
using CloudDataProtection.Core.DependencyInjection.Extensions;
using CloudDataProtection.Services.Subscription.Business;
using CloudDataProtection.Services.Subscription.Data.Context;
using CloudDataProtection.Services.Subscription.Seeder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CloudDataProtection.Services.Subscription
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args)
                .Build()
                .Migrate<IBackupConfigurationDbContext, BackupConfigurationDbContext>()
                .Seed()
                .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }

    public static class WebHostExtensions
    {
        public static IHost Seed(this IHost webHost)
        {
            using (IServiceScope scope = webHost.Services.CreateScope())
            {
                BackupSchemeBusinessLogic logic = scope.ServiceProvider.GetService<BackupSchemeBusinessLogic>();

                BackupSchemeSeeder seeder = new BackupSchemeSeeder(logic);

                Task task = seeder.Seed();

                task.Wait();
            }

            return webHost;
        }
    }
}