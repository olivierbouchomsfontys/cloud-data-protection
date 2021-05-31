using System.Threading.Tasks;
using CloudDataProtection.Entities;

namespace CloudDataProtection.Data
{
    public interface IUserHistoryRepository
    {
        Task RegisterDelete(UserDeletionHistory history);
        
        Task<UserDeletionHistory> GetDelete(long userId);
        Task Update(UserDeletionHistory history);
    }
}