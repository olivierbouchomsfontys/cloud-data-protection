using System.Collections.Generic;
using System.Threading.Tasks;
using CloudDataProtection.Core.Result;
using CloudDataProtection.Services.Subscription.Data.Repository;
using CloudDataProtection.Services.Subscription.Entities;

namespace CloudDataProtection.Services.Subscription.Business
{
    public class BackupSchemeBusinessLogic
    {
        private readonly IBackupSchemeRepository _repository;

        public BackupSchemeBusinessLogic(IBackupSchemeRepository repository)
        {
            _repository = repository;
        }

        public async Task<BusinessResult<BackupScheme>> Create(BackupScheme scheme)
        {
            await _repository.Create(scheme);
            
            return BusinessResult<BackupScheme>.Ok(scheme);
        }

        
        public async Task<BusinessResult<BackupScheme>> Get(long id)
        {
            BackupScheme scheme = await _repository.Get(id);

            if (scheme == null)
            {
                return BusinessResult<BackupScheme>.Error($"Could not find backup scheme with id = {id}");
            }
            
            return BusinessResult<BackupScheme>.Ok(scheme);
        }

        public async Task<BusinessResult<ICollection<BackupScheme>>> GetAll()
        {
            ICollection<BackupScheme> data = await _repository.GetAll();

            return BusinessResult<ICollection<BackupScheme>>.Ok(data);
        }
    }
}