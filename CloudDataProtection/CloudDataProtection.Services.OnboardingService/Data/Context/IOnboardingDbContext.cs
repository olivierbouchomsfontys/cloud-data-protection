using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CloudDataProtection.Services.Onboarding.Data.Context
{
    public interface IOnboardingDbContext
    {
        DbSet<Entities.Onboarding> Onboarding { get; set; }
        
        DbSet<Entities.GoogleCredentials> GoogleCredential { get; set; }
        
        DbSet<Entities.GoogleLoginToken> GoogleLoginToken { get; set; }

        Task<bool> SaveAsync();
    }
}