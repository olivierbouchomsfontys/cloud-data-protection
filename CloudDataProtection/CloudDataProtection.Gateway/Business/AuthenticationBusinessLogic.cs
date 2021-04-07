using System;
using System.Threading.Tasks;
using CloudDataProtection.Data;
using CloudDataProtection.Entities;

namespace CloudDataProtection.Business
{
    public class AuthenticationBusinessLogic
    {
        private readonly IAuthenticationRepository _repository;

        public AuthenticationBusinessLogic(IAuthenticationRepository repository)
        {
            _repository = repository;
        }

        // TODO Use businessResult
        public async Task<User> Authenticate(string username, string password)
        {
            var user = await _repository.Get(username);

            if (user == null)
            {
                return null;
            }

            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }

            return user;
        }

        public async Task<User> Get(int id)
        {
            return await _repository.Get(id);
        }

        // TODO Use BusinessResult
        public async Task<User> Create(User user, string password)
        {
            if (await _repository.Get(user.Email) != null)
            {
                // User exists
                return null;
            }

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _repository.Create(user);

            return user;
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            // TODO Use IPasswordHasher
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            // TODO Use IPasswordHasher
            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }
    }
}