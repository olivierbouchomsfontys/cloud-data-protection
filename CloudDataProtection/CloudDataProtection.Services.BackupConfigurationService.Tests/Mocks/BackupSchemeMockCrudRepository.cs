using CloudDataProtection.Services.Subscription.Data.Repository;
using CloudDataProtection.Services.Subscription.Entities;

namespace CloudDataProtection.Services.BackupConfigurationService.Tests.Mocks
{
    public class BackupSchemeMockCrudRepository : MockCrudRepositoryBase<BackupScheme>, IBackupSchemeRepository
    {
    }
}