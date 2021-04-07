using System;
using Microsoft.Extensions.DependencyInjection;

namespace CloudDataProtection.Core.DependencyInjection.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddLazy<TService>(this IServiceCollection services) where TService : class
        {
            services.AddTransient<TService>();
            services.AddTransient(provider => new Lazy<TService>(() => provider.GetRequiredService<TService>()));
        }
    }
}