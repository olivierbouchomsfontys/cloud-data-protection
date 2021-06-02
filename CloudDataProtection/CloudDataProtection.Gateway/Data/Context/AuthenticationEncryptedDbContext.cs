using System.IO;
using System.Threading.Tasks;
using CloudDataProtection.Core.Cryptography.Aes;
using CloudDataProtection.Core.Data.Context;
using CloudDataProtection.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CloudDataProtection.Data.Context
{
    public class AuthenticationEncryptedDbContext : EncryptedDbContextBase, IAuthenticationDbContext
    {
        public AuthenticationEncryptedDbContext()
        {
            
        }

        public AuthenticationEncryptedDbContext(DbContextOptions options) : base(options, null)
        {
            
        }

        public AuthenticationEncryptedDbContext(DbContextOptions options, ITransformer transformer) : base(options, transformer)
        {
            
        }
        
        public DbSet<User> User { get; set; }
        
        public async Task<bool> SaveAsync()
        {
            return await base.SaveChangesAsync() > 0;
        }

        protected sealed override void ConfigureForEfCoreTools(DbContextOptionsBuilder builder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json")
                .Build();
            
            string connectionString = configuration.GetConnectionString("DefaultConnection");

            builder.UseNpgsql(connectionString);
        }
    }
}