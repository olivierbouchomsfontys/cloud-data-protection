using System;
using CloudDataProtection.Core.Cryptography.Aes;
using CloudDataProtection.Core.Cryptography.Aes.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CloudDataProtection.Core.DependencyInjection.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddLazy<TImplementation>(this IServiceCollection services) where TImplementation : class
        {
            services.AddTransient<TImplementation>();
            services.AddTransient(provider => new Lazy<TImplementation>(() => provider.GetRequiredService<TImplementation>()));
        }
        
        public static void AddLazy<TService, TImplementation>(this IServiceCollection services) 
            where TService : class
            where TImplementation : class, TService
        {
            services.AddTransient<TService, TImplementation>();
            services.AddTransient(provider => new Lazy<TService>(() => provider.GetRequiredService<TService>()));
        }

        public static void AddEncryptedDbContext<TService, TImplementation>(this IServiceCollection services, IConfiguration configuration, Action<DbContextOptionsBuilder> optionsAction = null) 
            where TService : class 
            where TImplementation : DbContext, TService
        {
            services.Configure<AesOptions>(options => configuration.GetSection("Persistence").Bind(options));
            
            services.AddScoped<ITransformer, AesTransformer>();

            services.AddDbContext<TService, TImplementation>(optionsAction);
        }
    }
}