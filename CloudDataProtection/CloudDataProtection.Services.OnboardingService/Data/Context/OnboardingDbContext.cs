using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CloudDataProtection.Services.Onboarding.Data.Context
{
    public class OnboardingDbContext : DbContext, IOnboardingDbContext
    {
        public OnboardingDbContext()
        {
            
        }

        public OnboardingDbContext(DbContextOptions<OnboardingDbContext> options) : base(options)
        {
            
        }

        public OnboardingDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.Development.json")
                .Build();
            
            DbContextOptionsBuilder<OnboardingDbContext> builder = new DbContextOptionsBuilder<OnboardingDbContext>();
            
            string connectionString = configuration.GetConnectionString("DefaultConnection");

            builder.UseNpgsql(connectionString);

            return new OnboardingDbContext(builder.Options);
        }

        public DbSet<Entities.Onboarding> Onboarding { get; set; }
        
        public async Task<bool> SaveAsync()
        {
            return await SaveChangesAsync() > 0;
        }
    }
}