using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudDataProtection.Core.Entities;
using CloudDataProtection.Core.Seeder;
using CloudDataProtection.Services.Subscription.Business;
using CloudDataProtection.Services.Subscription.Entities;

namespace CloudDataProtection.Services.Subscription.Seeder
{
    public class BackupSchemeSeeder : ISeeder
    {
        private readonly BackupSchemeBusinessLogic _logic;

        private readonly List<BackupScheme> _schemes = new List<BackupScheme>(new[]
        {
            new BackupScheme
            {
                Frequency = BackupFrequency.Daily,
                Time = new Time(06, 00)
            }
        });

        public BackupSchemeSeeder(BackupSchemeBusinessLogic logic)
        {
            _logic = logic;
        }

        public async Task Seed()
        {
            var getResult = await _logic.GetAll();

            if (getResult.Success && getResult.Data.Any())
            {
                return;
            }

            foreach (BackupScheme backupScheme in _schemes)
            {
                await _logic.Create(backupScheme);
            }
        }
    }
}