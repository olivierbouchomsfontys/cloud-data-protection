using System.IO;
using System.Threading.Tasks;
using CloudDataProtection.Core.Cryptography.Aes;
using CloudDataProtection.Core.Data.Context;
using CloudDataProtection.Services.Subscription.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CloudDataProtection.Services.Subscription.Data.Context
{
    public class BackupConfigurationDbContext : EncryptedDbContextBase, IBackupConfigurationDbContext
    {
        public DbSet<BackupConfiguration> BackupConfiguration { get; set; }
        
        public DbSet<BackupScheme> BackupScheme { get; set; }

        public BackupConfigurationDbContext()
        {
            
        }

        public BackupConfigurationDbContext(DbContextOptions<BackupConfigurationDbContext> options, ITransformer transformer) : base(options, transformer)
        {
            
        }
        
        public async Task<bool> SaveAsync()
        {
            return await SaveChangesAsync() > 0;
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