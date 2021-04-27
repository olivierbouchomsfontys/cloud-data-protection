using System.Collections.Generic;
using System.Threading.Tasks;

namespace CloudDataProtection.Services.Onboarding.Data.Repository
{
    public interface IGoogleLoginTokenRepository
    {
        Task Create(Entities.GoogleLoginToken token);
        
        Task<Entities.GoogleLoginToken> Get(long id);
        
        Task<Entities.GoogleLoginToken> Get(string token);
 
        Task<ICollection<Entities.GoogleLoginToken>> GetAllByUser(long userId);

        Task Update(Entities.GoogleLoginToken token);
        
        Task Update(ICollection<Entities.GoogleLoginToken> tokens);
    }
}