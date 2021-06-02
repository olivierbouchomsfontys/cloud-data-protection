using System.Collections.Generic;
using System.Threading.Tasks;
using CloudDataProtection.Services.Onboarding.Entities;

namespace CloudDataProtection.Services.Onboarding.Data.Repository
{
    public interface IGoogleLoginTokenRepository
    {
        Task Create(GoogleLoginToken token);
        
        Task<GoogleLoginToken> Get(long id);
        
        Task<GoogleLoginToken> Get(string token);
 
        Task<ICollection<GoogleLoginToken>> GetAllByUser(long userId);

        Task Update(GoogleLoginToken token);
        Task Update(ICollection<GoogleLoginToken> tokens);
        
        Task Delete(ICollection<GoogleLoginToken> tokens);
    }
}