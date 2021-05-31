using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudDataProtection.Services.Onboarding.Data.Context;
using CloudDataProtection.Services.Onboarding.Entities;
using Microsoft.EntityFrameworkCore;

namespace CloudDataProtection.Services.Onboarding.Data.Repository
{
    public class LoginTokenRepository : IGoogleLoginTokenRepository
    {
        private readonly IOnboardingDbContext _context;

        public LoginTokenRepository(IOnboardingDbContext context)
        {
            _context = context;
        }
        
        public async Task Create(GoogleLoginToken token)
        {
            await _context.GoogleLoginToken.AddAsync(token);

            await _context.SaveAsync();
        }

        public async Task<GoogleLoginToken> Get(long id)
        {
            return await _context.GoogleLoginToken.FindAsync(id);
        }

        public async Task<GoogleLoginToken> Get(string token)
        {
            return await _context.GoogleLoginToken.FirstOrDefaultAsync(g => g.Token == token);
        }

        public async Task<ICollection<GoogleLoginToken>> GetAllByUser(long userId)
        {
            return await _context.GoogleLoginToken
                .Where(g => g.UserId == userId)
                .ToArrayAsync();
        }

        public async Task Update(GoogleLoginToken token)
        {
            _context.GoogleLoginToken.Update(token);

            await _context.SaveAsync();
        }

        public async Task Update(ICollection<GoogleLoginToken> tokens)
        {
            _context.GoogleLoginToken.UpdateRange(tokens);

            await _context.SaveAsync();
        }

        public async Task Delete(ICollection<GoogleLoginToken> tokens)
        {
            _context.GoogleLoginToken.RemoveRange(tokens);

            await _context.SaveAsync();
        }
    }
}