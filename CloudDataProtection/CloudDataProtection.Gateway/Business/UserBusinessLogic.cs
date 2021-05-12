using System.Threading.Tasks;
using CloudDataProtection.Core.Result;
using CloudDataProtection.Data;
using CloudDataProtection.Entities;

namespace CloudDataProtection.Business
{
    public class UserBusinessLogic
    {
        private readonly IAuthenticationRepository _repository;

        public UserBusinessLogic(IAuthenticationRepository repository)
        {
            _repository = repository;
        }
        
        public async Task<BusinessResult<User>> Get(long id)
        {
            User user = await _repository.Get(id);

            if (user == null)
            {
                return BusinessResult<User>.Error($"Could not find user with id = {id}");
            }
            
            return BusinessResult<User>.Ok(user);
        }
    }
}