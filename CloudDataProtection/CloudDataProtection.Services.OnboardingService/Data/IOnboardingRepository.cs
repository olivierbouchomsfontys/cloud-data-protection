using System.Threading.Tasks;

namespace CloudDataProtection.Services.Onboarding.Data
{
    public interface IOnboardingRepository
    {
        Task Create(Entities.Onboarding onboarding);

        Task<Entities.Onboarding> Get(int id);

        Task<Entities.Onboarding> GetByUserId(int userId);
    }
}