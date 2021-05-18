using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CloudDataProtection.Core.Cryptography.Aes;
using CloudDataProtection.Core.Cryptography.Attributes;
using CloudDataProtection.Services.Onboarding.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace CloudDataProtection.Services.Onboarding.Data.Context
{
    public class OnboardingDbContext : DbContext, IOnboardingDbContext
    {
        public static ITransformer Transformer { private get; set; }
        
        public DbSet<Entities.Onboarding> Onboarding { get; set; }
        
        public DbSet<GoogleCredentials> GoogleCredential { get; set; }
        
        public DbSet<GoogleLoginToken> GoogleLoginToken { get; set; }

        public OnboardingDbContext()
        {
        }

        public OnboardingDbContext(DbContextOptions<OnboardingDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (IMutableProperty property in entityType.GetProperties())
                {
                    object[] attributes = property.PropertyInfo.GetCustomAttributes(typeof(EncryptAttribute), false);
                    
                    if (attributes.Any())
                    {
                        property.SetValueConverter(new AesValueConverter(Transformer));
                    }
                }
            }
        }

        public async Task<bool> SaveAsync()
        {
            return await SaveChangesAsync() > 0;
        }
    }
}