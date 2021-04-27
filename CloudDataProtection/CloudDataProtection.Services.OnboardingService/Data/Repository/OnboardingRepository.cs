using System.Threading.Tasks;
using CloudDataProtection.Services.Onboarding.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace CloudDataProtection.Services.Onboarding.Data.Repository
{
    public class OnboardingRepository : IOnboardingRepository
    {
        private readonly IOnboardingDbContext _context;
        
        public OnboardingRepository(IOnboardingDbContext context)
        {
            _context = context;
        }
        
        public async Task Create(Entities.Onboarding onboarding)
        {
            await _context.Onboarding.AddAsync(onboarding);

            await _context.SaveAsync();
        }

        public async Task<Entities.Onboarding> Get(long id)
        {
            return await _context.Onboarding.FindAsync(id);
        }

        public async Task<Entities.Onboarding> GetByUserId(long userId)
        {
            return await _context.Onboarding.FirstOrDefaultAsync(o => o.UserId == userId);
        }

        public async Task Update(Entities.Onboarding onboarding)
        {
            _context.Onboarding.Update(onboarding);

            await _context.SaveAsync();
        }
    }
}