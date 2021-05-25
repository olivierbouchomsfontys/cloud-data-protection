using System.Collections.Generic;
using System.Threading.Tasks;
using CloudDataProtection.Core.Entities;
using CloudDataProtection.Core.Result;
using CloudDataProtection.Services.BackupConfigurationService.Tests.Mocks;
using CloudDataProtection.Services.Subscription.Business;
using CloudDataProtection.Services.Subscription.Entities;
using Xunit;

namespace CloudDataProtection.Services.BackupConfigurationService.Tests.Business
{
    public class BackupSchemeBusinessLogicTests
    {
        private BackupSchemeBusinessLogic _logic;
        
        public BackupSchemeBusinessLogicTests()
        {
            _logic = new BackupSchemeBusinessLogic(new BackupSchemeMockCrudRepository());
        }

        private void Seed(IEnumerable<BackupScheme> data)
        {
            BackupSchemeMockCrudRepository crudRepository = new BackupSchemeMockCrudRepository();
            
            crudRepository.Seed(data);

            _logic = new BackupSchemeBusinessLogic(crudRepository);

        }
        
        [Fact]
        public async Task TestCreate_Valid()
        {
            BackupScheme scheme = new BackupScheme
            {
                Frequency = BackupFrequency.Daily,
                Time = new Time(18, 00),
            };

            BusinessResult<BackupScheme> result = await _logic.Create(scheme);
            
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.True(result.Data.Id > 0);
            Assert.Equal(scheme.Frequency, result.Data.Frequency);
            Assert.Equal(scheme.Time.Hours, result.Data.Time.Hours);
            Assert.Equal(scheme.Time.Minutes, result.Data.Time.Minutes);
        }

        [Fact]
        public async Task TestGet_Valid()
        {
            BackupScheme seed = new BackupScheme
            {
                Frequency = BackupFrequency.Daily,
                Time = new Time(09, 00)
            };

            Seed(new []{ seed });

            long id = 1;

            BusinessResult<BackupScheme> result = await _logic.Get(id);
            
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(seed.Frequency, result.Data.Frequency);
            Assert.Equal(seed.Time.Hours, result.Data.Time.Hours);
            Assert.Equal(seed.Time.Minutes, result.Data.Time.Minutes);
        } 

        [Fact]
        public async Task TestGet_Invalid()
        {
            BackupScheme seed = new BackupScheme
            {
                Frequency = BackupFrequency.Daily,
                Time = new Time(09, 00)
            };

            Seed(new []{ seed });

            long id = 9999;

            BusinessResult<BackupScheme> result = await _logic.Get(id);
            
            Assert.False(result.Success);
            Assert.Null(result.Data);
            Assert.NotEmpty(result.Message);
        } 

        [Fact]
        public async Task TestGetAll_Empty_Valid()
        {
            BusinessResult<ICollection<BackupScheme>> result = await _logic.GetAll();
            
            Assert.True(result.Success);
            Assert.Empty(result.Data);
        }

        [Fact]
        public async Task TestGetAll_WithData_Valid()
        {
            IList<BackupScheme> schemes = new List<BackupScheme>();
            
            schemes.Add(new BackupScheme
            {
                Frequency = BackupFrequency.Daily,
                Time = new Time(08, 00),
            });
            schemes.Add(new BackupScheme
            {
                Frequency = BackupFrequency.Daily,
                Time = new Time(14, 00),
            });
            schemes.Add(new BackupScheme
            {
                Frequency = BackupFrequency.Daily,
                Time = new Time(18, 00),
            });
            
            Seed(schemes);

            var result = await _logic.GetAll();
            
            Assert.True(result.Success);
            Assert.NotEmpty(result.Data);
            Assert.Equal(schemes.Count, result.Data.Count);
        }
    }
}