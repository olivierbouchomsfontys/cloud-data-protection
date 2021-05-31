using System.Threading.Tasks;
using CloudDataProtection.Data.Context;
using CloudDataProtection.Entities;
using Microsoft.EntityFrameworkCore;

namespace CloudDataProtection.Data
{
    public class UserHistoryRepository : IUserHistoryRepository
    {
        private IAuthenticationDbContext _context;

        public UserHistoryRepository(IAuthenticationDbContext context)
        {
            _context = context;
        }

        public async Task RegisterDelete(UserDeletionHistory history)
        {
            _context.UserDeletionHistory.Add(history);

            await _context.SaveAsync();
        }

        public async Task<UserDeletionHistory> GetDelete(long userId)
        {
            return await _context.UserDeletionHistory
                .Include(h => h.Progress)
                .FirstOrDefaultAsync(h => h.UserId == userId);
        }

        public async Task Update(UserDeletionHistory history)
        {
            _context.UserDeletionHistory.Update(history);

            await _context.SaveAsync();
        }
    }
}