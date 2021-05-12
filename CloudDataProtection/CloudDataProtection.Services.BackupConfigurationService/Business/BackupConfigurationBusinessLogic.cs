using System.Threading.Tasks;
using CloudDataProtection.Core.Result;
using CloudDataProtection.Services.Subscription.Data.Repository;
using CloudDataProtection.Services.Subscription.Entities;

namespace CloudDataProtection.Services.Subscription.Business
{
    public class BackupConfigurationBusinessLogic
    {
        private readonly IBackupConfigurationRepository _repository;

        public BackupConfigurationBusinessLogic(IBackupConfigurationRepository repository)
        {
            _repository = repository;
        }

        public async Task<BusinessResult<BackupConfiguration>> Create(BackupConfiguration configuration)
        {
            if (configuration.UserId <= 0)
            {
                return BusinessResult<BackupConfiguration>.Error("User id was not set for backup configuration");
            }

            BusinessResult<BackupConfiguration> existing = await GetByUser(configuration.UserId);

            if (existing.Data != null)
            {
                return BusinessResult<BackupConfiguration>.Error("Backup configuration is already set", existing.Data);
            }

            await _repository.Create(configuration);
            
            return BusinessResult<BackupConfiguration>.Ok(configuration);
        }

        public async Task<BusinessResult<BackupConfiguration>> GetByUser(long userId)
        {
            BackupConfiguration configuration = await _repository.GetByUser(userId);

            if (configuration == null)
            {
                return BusinessResult<BackupConfiguration>.Error($"Could not find backup configuration for user with id = {userId}");
            }
            
            return BusinessResult<BackupConfiguration>.Ok(configuration);
        }
    }
}