using System.IO;
using System.Threading.Tasks;
using CloudDataProtection.Core.Cryptography.Aes;
using CloudDataProtection.Core.Data.Context;
using CloudDataProtection.Services.Onboarding.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CloudDataProtection.Services.Onboarding.Data.Context
{
    public class OnboardingDbContext : EncryptedDbContextBase, IOnboardingDbContext
    {
        public DbSet<Entities.Onboarding> Onboarding { get; set; }
        
        public DbSet<GoogleCredentials> GoogleCredential { get; set; }
        
        public DbSet<GoogleLoginToken> GoogleLoginToken { get; set; }

        public OnboardingDbContext()
        {
        }

        public OnboardingDbContext(DbContextOptions<OnboardingDbContext> options, ITransformer transformer) : base(options, transformer)
        {
            
        }

        public async Task<bool> SaveAsync()
        {
            return await SaveChangesAsync() > 0;
        }

        protected sealed override void ConfigureForEfCoreTools(DbContextOptionsBuilder builder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json")
                .Build();
            
            string connectionString = configuration.GetConnectionString("DefaultConnection");

            builder.UseNpgsql(connectionString);
        }

    }
}