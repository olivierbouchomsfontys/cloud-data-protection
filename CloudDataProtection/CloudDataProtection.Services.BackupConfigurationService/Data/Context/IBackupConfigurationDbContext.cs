using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CloudDataProtection.Services.Subscription.Data.Context
{
    public interface IBackupConfigurationDbContext
    {
        DbSet<Entities.BackupConfiguration> BackupConfiguration { get; set; }
        DbSet<Entities.BackupScheme> BackupScheme { get; set; }

        Task<bool> SaveAsync();
    }
}