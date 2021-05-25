using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudDataProtection.Core.Entities;

namespace CloudDataProtection.Services.BackupConfigurationService.Tests.Mocks
{
    public abstract class MockCrudRepositoryBase<TEntity> where TEntity : IEntity<long>
    {
        protected IList<TEntity> _data = new List<TEntity>();

        public MockCrudRepositoryBase() { }

        public Task<TEntity> Get(long id) => Task.FromResult(_data.FirstOrDefault(e => e.Id == id));

        public Task<ICollection<TEntity>> GetAll() => Task.FromResult<ICollection<TEntity>>(_data.ToArray());

        public Task Create(TEntity entity)
        {
            entity.Id = _data.Count + 1;
            
            _data.Add(entity);
            
            return Task.CompletedTask;
        }

        public void Seed(IEnumerable<TEntity> data)
        {
            foreach (TEntity entity in data)
            {
                Create(entity).Wait();
            }
        }

        public void Reset() => _data.Clear();
    }
}