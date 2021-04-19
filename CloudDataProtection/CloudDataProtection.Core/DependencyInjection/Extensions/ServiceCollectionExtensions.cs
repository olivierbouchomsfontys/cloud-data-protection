using System;
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
    }
}