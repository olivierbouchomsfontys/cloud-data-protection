using System.Threading.Tasks;
using CloudDataProtection.Services.Onboarding.Entities;

namespace CloudDataProtection.Services.Onboarding.Data.Repository
{
    public interface IGoogleCredentialsRepository
    {
        Task Create(GoogleCredentials credentials);

        Task<GoogleCredentials> Get(int id);
        
        Task<GoogleCredentials> GetByUserId(long userId);
        
        Task Delete(GoogleCredentials credentials);
    }
}