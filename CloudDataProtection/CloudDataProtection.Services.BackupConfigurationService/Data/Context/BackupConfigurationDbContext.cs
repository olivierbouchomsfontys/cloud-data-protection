using System.IO;
using System.Threading.Tasks;
using CloudDataProtection.Services.Subscription.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CloudDataProtection.Services.Subscription.Data.Context
{
    public class BackupConfigurationDbContext : DbContext, IBackupConfigurationDbContext
    {
        public DbSet<BackupConfiguration> BackupConfiguration { get; set; }
        
        public DbSet<BackupScheme> BackupScheme { get; set; }

        public BackupConfigurationDbContext()
        {
            
        }

        public BackupConfigurationDbContext(DbContextOptions<BackupConfigurationDbContext> options) : base(options)
        {
            
        }
        
        public BackupConfigurationDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.Development.json")
                .Build();

            DbContextOptionsBuilder<BackupConfigurationDbContext> builder =
                new DbContextOptionsBuilder<BackupConfigurationDbContext>();

            string connectionString = configuration.GetConnectionString("DefaultConnection");

            builder.UseNpgsql(connectionString);

            return new BackupConfigurationDbContext(builder.Options);
        }
        
        public async Task<bool> SaveAsync()
        {
            return await SaveChangesAsync() > 0;
        }
    }
}