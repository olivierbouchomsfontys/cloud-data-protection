using System.Collections.Generic;
using System.Threading.Tasks;
using CloudDataProtection.Core.Entities;
using CloudDataProtection.Core.Result;
using CloudDataProtection.Services.BackupConfigurationService.Tests.Mocks;
using CloudDataProtection.Services.Subscription.Business;
using CloudDataProtection.Services.Subscription.Entities;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CloudDataProtection.Services.BackupConfigurationService.Tests.Business
{
    public class BackupConfigurationBusinessLogicTests
    {
        private BackupConfigurationBusinessLogic _logic;

        public BackupConfigurationBusinessLogicTests()
        {
            _logic = new BackupConfigurationBusinessLogic(new BackupConfigurationMockRepository());
        }

        private void Seed(IEnumerable<BackupConfiguration> data)
        {
            BackupConfigurationMockRepository repository = new BackupConfigurationMockRepository();
            
            repository.Seed(data);

            _logic = new BackupConfigurationBusinessLogic(repository);
        }

        [Fact]
        public async Task TestCreate_Valid()
        {
            BackupConfiguration configuration = new BackupConfiguration
            {
                Frequency = BackupFrequency.Daily,
                Time = new Time(18, 00),
                UserId = 1
            };

            BusinessResult<BackupConfiguration> result = await _logic.Create(configuration);
            
            Assert.True(result.Success);
            Assert.Equal(configuration.Frequency, result.Data.Frequency);
            Assert.Equal(configuration.Time.Hours, result.Data.Time.Hours);
            Assert.Equal(configuration.Time.Minutes, result.Data.Time.Minutes);
            Assert.Equal(configuration.UserId, result.Data.UserId);
        }

        [Fact]
        public async Task TestCreate_Invalid()
        {
            BackupConfiguration configuration = new BackupConfiguration
            {
                Frequency = BackupFrequency.Daily,
                Time = new Time(18, 00),
                UserId = -1
            };

            BusinessResult<BackupConfiguration> result = await _logic.Create(configuration);
            
            Assert.False(result.Success);
            Assert.Null(result.Data);
            Assert.NotEmpty(result.Message);
        }

        [Fact]
        public async Task TestGetByUser_Valid()
        {
            BackupConfiguration configuration = new BackupConfiguration
            {
                Frequency = BackupFrequency.Daily,
                Time = new Time(18, 00),
                UserId = 1
            };

            Seed(new []{configuration});
            
            BusinessResult<BackupConfiguration> result = await _logic.GetByUser(configuration.UserId);
            
            Assert.True(result.Success);
            Assert.Equal(configuration.Frequency, result.Data.Frequency);
            Assert.Equal(configuration.Time.Hours, result.Data.Time.Hours);
            Assert.Equal(configuration.Time.Minutes, result.Data.Time.Minutes);
            Assert.Equal(configuration.UserId, result.Data.UserId);
        }

        [Fact]
        public async Task TestGetByUser_Invalid()
        {
            BackupConfiguration configuration = new BackupConfiguration
            {
                Frequency = BackupFrequency.Daily,
                Time = new Time(18, 00),
                UserId = 1
            };

            Seed(new []{configuration});
            
            BusinessResult<BackupConfiguration> result = await _logic.GetByUser(-1);
            
            Assert.False(result.Success);
            Assert.Null(result.Data);
            Assert.NotEmpty(result.Message);
        }
    }
}