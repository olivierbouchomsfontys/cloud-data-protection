using System.Threading.Tasks;
using CloudDataProtection.Services.Subscription.Entities;
using Microsoft.EntityFrameworkCore;

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
        
        public async Task<bool> SaveAsync()
        {
            return await SaveChangesAsync() > 0;
        }
    }
}