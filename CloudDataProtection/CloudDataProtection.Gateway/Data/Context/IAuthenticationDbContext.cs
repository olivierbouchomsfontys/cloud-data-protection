using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CloudDataProtection.Data.Context
{
    public interface IAuthenticationDbContext : IDesignTimeDbContextFactory<AuthenticationDbContext>
    {
        DbSet<Entities.User> User { get; set; }

        Task<bool> SaveAsync();
    }
}