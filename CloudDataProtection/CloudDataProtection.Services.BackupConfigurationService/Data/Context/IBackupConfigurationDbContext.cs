using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CloudDataProtection.Services.Subscription.Data.Context
{
    public interface IBackupConfigurationDbContext : IDesignTimeDbContextFactory<BackupConfigurationDbContext>
    {
        DbSet<Entities.BackupConfiguration> BackupConfiguration { get; set; }
        DbSet<Entities.BackupScheme> BackupScheme { get; set; }

        Task<bool> SaveAsync();
    }
}