using System.Threading.Tasks;
using CloudDataProtection.Data.Context;
using CloudDataProtection.Entities;
using Microsoft.EntityFrameworkCore;

namespace CloudDataProtection.Data
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly IAuthenticationDbContext _context;
        
        public AuthenticationRepository(IAuthenticationDbContext context)
        {
            _context = context;
        }
        
        public async Task<User> Get(long id)
        {
            return await _context.User.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> Get(string email)
        {
            return await _context.User.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task Create(User user)
        {
            _context.User.Add(user);

            await _context.SaveAsync();
        }
    }
}