using System.Collections.Generic;
using System.Threading.Tasks;
using CloudDataProtection.Services.Subscription.Entities;

namespace CloudDataProtection.Services.Subscription.Data.Repository
{
    public interface IBackupSchemeRepository
    {
        Task Create(BackupScheme scheme);

        Task<BackupScheme> Get(long id);

        Task<ICollection<BackupScheme>> GetAll();
    }
}