using System.Threading.Tasks;
using CloudDataProtection.Services.Subscription.Entities;

namespace CloudDataProtection.Services.Subscription.Data.Repository
{
    public interface IBackupConfigurationRepository
    {
        Task Create(BackupConfiguration configuration);
        
        Task<BackupConfiguration> Get(long id);
        
        Task<BackupConfiguration> GetByUser(long userId);
    }
}