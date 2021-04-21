using System.IO;
using System.Threading.Tasks;
using CloudDataProtection.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

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
        
        public AuthenticationDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.Development.json")
                .Build();
            
            DbContextOptionsBuilder<AuthenticationDbContext> builder = new DbContextOptionsBuilder<AuthenticationDbContext>();
            
            string connectionString = configuration.GetConnectionString("DefaultConnection");

            builder.UseNpgsql(connectionString);

            return new AuthenticationDbContext(builder.Options);
        }

        public DbSet<User> User { get; set; }
        
        public async Task<bool> SaveAsync()
        {
            return await base.SaveChangesAsync() > 0;
        }
    }
}