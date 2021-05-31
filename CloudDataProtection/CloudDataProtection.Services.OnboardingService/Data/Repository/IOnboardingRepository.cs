using System.Threading.Tasks;

namespace CloudDataProtection.Services.Onboarding.Data.Repository
{
    public interface IOnboardingRepository
    {
        Task Create(Entities.Onboarding onboarding);

        Task<Entities.Onboarding> Get(long id);

        Task<Entities.Onboarding> GetByUserId(long userId);
        
        Task Update(Entities.Onboarding onboarding);
        
        Task Delete(Entities.Onboarding onboarding);
    }
}