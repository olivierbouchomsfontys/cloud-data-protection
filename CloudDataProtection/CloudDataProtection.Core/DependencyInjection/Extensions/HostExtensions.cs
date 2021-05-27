using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CloudDataProtection.Core.DependencyInjection.Extensions
{
    public static class HostExtensions
    {
        public static IHost Migrate<TDbContext, TDbContextImplementation>(this IHost webHost) where TDbContextImplementation : DbContext, TDbContext
        {
            using (IServiceScope scope = webHost.Services.CreateScope())
            {
                ILogger<TDbContext> logger = scope.ServiceProvider.GetRequiredService<ILogger<TDbContext>>();
                
                TDbContextImplementation context = scope.ServiceProvider.GetRequiredService<TDbContext>() as TDbContextImplementation;
                
                logger.LogInformation("Running migrations on {DbContext}", context.GetType().Name);

                context.Database.Migrate();
                
                logger.LogInformation("Completed migrations on {DbContext}", context.GetType().Name);
            }

            return webHost;
        }
    }
}