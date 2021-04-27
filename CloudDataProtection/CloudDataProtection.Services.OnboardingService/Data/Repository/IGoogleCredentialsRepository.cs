using System.Threading.Tasks;

namespace CloudDataProtection.Services.Onboarding.Data.Repository
{
    public interface IGoogleCredentialsRepository
    {
        Task Create(Entities.GoogleCredentials credentials);

        Task<Entities.GoogleCredentials> Get(int id);
        
        Task<Entities.GoogleCredentials> GetByUserId(long userId);
    }
}