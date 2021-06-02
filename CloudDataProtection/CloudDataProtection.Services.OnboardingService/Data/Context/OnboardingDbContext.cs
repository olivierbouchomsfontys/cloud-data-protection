using System.Threading.Tasks;
using CloudDataProtection.Core.Data.Context;
using CloudDataProtection.Services.Onboarding.Entities;
using Microsoft.EntityFrameworkCore;

namespace CloudDataProtection.Services.Onboarding.Data.Context
{
    public class OnboardingDbContext : DbContextBase, IOnboardingDbContext
    {
        public DbSet<Entities.Onboarding> Onboarding { get; set; }
        
        public DbSet<GoogleCredentials> GoogleCredential { get; set; }
        
        public DbSet<GoogleLoginToken> GoogleLoginToken { get; set; }

        public OnboardingDbContext()
        {
        }

        public OnboardingDbContext(DbContextOptions<OnboardingDbContext> options) : base(options)
        {
            
        }

        public async Task<bool> SaveAsync()
        {
            return await SaveChangesAsync() > 0;
        }
    }
}