using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CloudDataProtection.Services.Onboarding.Data.Context
{
    public interface IOnboardingDbContext : IDesignTimeDbContextFactory<OnboardingDbContext>
    {
        DbSet<Entities.Onboarding> Onboarding { get; set; }

        Task<bool> SaveAsync();
    }
}