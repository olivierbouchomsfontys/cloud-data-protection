using System.Threading.Tasks;
using CloudDataProtection.Core.Result;
using CloudDataProtection.Services.Onboarding.Data;
using CloudDataProtection.Services.Onboarding.Entities;
using Microsoft.Extensions.Logging;

namespace CloudDataProtection.Services.Onboarding.Business
{
    public class OnboardingBusinessLogic
    {
        private readonly IOnboardingRepository _repository;
        private readonly ILogger<OnboardingBusinessLogic> _logger;

        public OnboardingBusinessLogic(IOnboardingRepository repository, ILogger<OnboardingBusinessLogic> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        
        public async Task<BusinessResult<bool>> IsOnboarded(int userId)
        {
            Entities.Onboarding onboarding = await _repository.GetByUserId(userId);

            if (onboarding == null)
            {
                return BusinessResult<bool>.Error($"Could not find onboarding for user with id = {userId}");
            }
            
            return BusinessResult<bool>.Ok(onboarding.Status == OnboardingStatus.Complete);
        }

        public async Task<BusinessResult<Entities.Onboarding>> Create(Entities.Onboarding onboarding)
        {
            if (onboarding.UserId == 0)
            {
                return BusinessResult<Entities.Onboarding>.Error("User Id was not set for onboarding");
            }

            await _repository.Create(onboarding);
            
            return BusinessResult<Entities.Onboarding>.Ok(onboarding);
        }
    }
}