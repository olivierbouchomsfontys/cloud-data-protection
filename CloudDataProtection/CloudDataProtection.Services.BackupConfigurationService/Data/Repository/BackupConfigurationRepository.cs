using System.Threading.Tasks;
using CloudDataProtection.Services.Subscription.Data.Context;
using CloudDataProtection.Services.Subscription.Entities;
using Microsoft.EntityFrameworkCore;

namespace CloudDataProtection.Services.Subscription.Data.Repository
{
    public class BackupConfigurationRepository : IBackupConfigurationRepository
    {
        private readonly IBackupConfigurationDbContext _context;

        public BackupConfigurationRepository(IBackupConfigurationDbContext context)
        {
            _context = context;
        }

        public async Task Create(BackupConfiguration configuration)
        {
            await _context.BackupConfiguration.AddAsync(configuration);

            await _context.SaveAsync();
        }

        public async Task<BackupConfiguration> Get(long id)
        {
            return await _context.BackupConfiguration
                .Include(b => b.Time)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<BackupConfiguration> GetByUser(long userId)
        {
            return await _context.BackupConfiguration
                .Include(b => b.Time)
                .FirstOrDefaultAsync(b => b.UserId == userId);
        }
    }
}