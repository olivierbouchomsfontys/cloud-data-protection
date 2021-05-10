using System.Collections.Generic;
using System.Threading.Tasks;
using CloudDataProtection.Services.Subscription.Data.Context;
using CloudDataProtection.Services.Subscription.Entities;
using Microsoft.EntityFrameworkCore;

namespace CloudDataProtection.Services.Subscription.Data.Repository
{
    public class BackupSchemeRepository : IBackupSchemeRepository
    {
        private readonly IBackupConfigurationDbContext _context;

        public BackupSchemeRepository(IBackupConfigurationDbContext context)
        {
            _context = context;
        }
        
        public async Task Create(BackupScheme scheme)
        {
            await _context.BackupScheme.AddAsync(scheme);

            await _context.SaveAsync();
        }

        public async Task<BackupScheme> Get(long id)
        {
            return await _context.BackupScheme
                .Include(b => b.Time)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<ICollection<BackupScheme>> GetAll()
        {
            return await _context.BackupScheme
                .Include(b => b.Time)
                .ToArrayAsync();
        }
    }
}