using System.Threading.Tasks;
using CloudDataProtection.Entities;
using Microsoft.EntityFrameworkCore;

namespace CloudDataProtection.Data.Context
{
    public class AuthenticationDbContext : DbContext, IAuthenticationDbContext
    {
        public AuthenticationDbContext()
        {
            
        }

        public AuthenticationDbContext(DbContextOptions options) : base(options)
        {
            
        }
        
        public DbSet<User> User { get; set; }
        
        public async Task<bool> SaveAsync()
        {
            return await base.SaveChangesAsync() > 0;
        }
    }
}