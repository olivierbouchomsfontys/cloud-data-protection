using System.Threading.Tasks;
using CloudDataProtection.Services.Onboarding.Data.Context;
using CloudDataProtection.Services.Onboarding.Entities;
using Microsoft.EntityFrameworkCore;

namespace CloudDataProtection.Services.Onboarding.Data.Repository
{
    public class GoogleCredentialsRepository : IGoogleCredentialsRepository
    {
        private readonly IOnboardingDbContext _context;

        public GoogleCredentialsRepository(IOnboardingDbContext context)
        {
            _context = context;
        }
        
        public async Task Create(GoogleCredentials credentials)
        {
            _context.GoogleCredential.Add(credentials);

            await _context.SaveAsync();
        }

        public async Task<GoogleCredentials> Get(int id)
        {
            return await _context.GoogleCredential.FindAsync(id);
        }

        public async Task<GoogleCredentials> GetByUserId(long userId)
        {
            return await _context.GoogleCredential.FirstOrDefaultAsync(o => o.UserId == userId);
        }

        public async Task Delete(GoogleCredentials credentials)
        {
            _context.GoogleCredential.Remove(credentials);

            await _context.SaveAsync();
        }
    }
}