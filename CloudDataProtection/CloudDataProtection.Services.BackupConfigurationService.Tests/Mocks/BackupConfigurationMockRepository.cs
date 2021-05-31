using System.Linq;
using System.Threading.Tasks;
using CloudDataProtection.Services.Subscription.Data.Repository;
using CloudDataProtection.Services.Subscription.Entities;

namespace CloudDataProtection.Services.BackupConfigurationService.Tests.Mocks
{
    public class BackupConfigurationMockRepository : MockCrudRepositoryBase<BackupConfiguration>, IBackupConfigurationRepository
    {
        public Task<BackupConfiguration> GetByUser(long userId) =>
            Task.FromResult(_data.FirstOrDefault(e => e.UserId == userId));

        public Task Delete(BackupConfiguration configuration)
        {
            _data.Remove(configuration);
            
            return Task.CompletedTask;
        }
    }
}