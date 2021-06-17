using System.Collections.Generic;
using System.Threading.Tasks;
using CloudDataProtection.Entities;

namespace CloudDataProtection.Data
{
    public interface IAuthenticationRepository
    {
        Task<User> Get(long id);
        Task<User> Get(string email);

        Task Create(User user);
        Task Delete(User user);

        Task Create(ChangeEmailRequest request);
        Task Update(ChangeEmailRequest request);
        Task Update(IEnumerable<ChangeEmailRequest> requests);

        Task<IEnumerable<ChangeEmailRequest>> GetAll(long userId);
    }
}