using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using CloudDataProtection.Core.Result;
using CloudDataProtection.Data;
using CloudDataProtection.Entities;
using CloudDataProtection.Password;

namespace CloudDataProtection.Business
{
    public class AuthenticationBusinessLogic
    {
        private readonly IAuthenticationRepository _repository;
        private readonly IPasswordHasher _passwordHasher;

        private const int MinimumPasswordLength = 8;

        public AuthenticationBusinessLogic(IAuthenticationRepository repository, IPasswordHasher passwordHasher)
        {
            _repository = repository;
            _passwordHasher = passwordHasher;
        }

        public async Task<BusinessResult<User>> Authenticate(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return BusinessResult<User>.Error("Invalid username or password");
            }
            
            User user = await _repository.Get(username);

            if (user == null)
            {
                return BusinessResult<User>.Error("Invalid username or password");
            }

            if (!_passwordHasher.Match(user.Password, password))
            {
                return BusinessResult<User>.Error("Invalid username or password");
            }

            return BusinessResult<User>.Ok(user);
        }

        public async Task<BusinessResult<User>> Get(int id)
        {
            User user = await _repository.Get(id);

            if (user == null)
            {
                return BusinessResult<User>.Error($"Could not find user with id = {id}");
            }
            
            return BusinessResult<User>.Ok(user);
        }

        public async Task<BusinessResult<User>> Create(User user, string password)
        {
            if (user.Email == null || !new EmailAddressAttribute().IsValid(user.Email))
            {
                return BusinessResult<User>.Error("Invalid email provided");
            }

            if (string.IsNullOrWhiteSpace(password) || password.Length < MinimumPasswordLength)
            {
                return BusinessResult<User>.Error($"Password must be at least {MinimumPasswordLength} characters long");
            }
            
            if (await _repository.Get(user.Email) != null)
            {
                return BusinessResult<User>.Error($"A user with email {user.Email} already exists");
            }

            user.Password = _passwordHasher.HashPassword(password);
            
            await _repository.Create(user);

            return BusinessResult<User>.Ok(user);
        }
    }
}