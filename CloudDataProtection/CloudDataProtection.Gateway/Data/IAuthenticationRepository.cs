using System.Threading.Tasks;
using CloudDataProtection.Entities;

namespace CloudDataProtection.Data
{
    public interface IAuthenticationRepository
    {
        Task<User> Get(int id);
        Task<User> Get(string email);

        Task Create(User user);
    }
}