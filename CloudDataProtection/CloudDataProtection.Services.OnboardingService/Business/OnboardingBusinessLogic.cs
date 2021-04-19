﻿using System.Threading.Tasks;
using CloudDataProtection.Core.Result;
using CloudDataProtection.Services.Onboarding.Data;
using CloudDataProtection.Services.Onboarding.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;

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

        public async Task<BusinessResult<Entities.Onboarding>> GetByUser(long userId)
        {
            Entities.Onboarding onboarding = await _repository.GetByUserId(userId);

            if (onboarding == null)
            {
                Entities.Onboarding newOnboarding = new Entities.Onboarding
                {
                    UserId = userId
                };

                await DoCreate(newOnboarding);

                return await GetByUser(userId);
            }

            return BusinessResult<Entities.Onboarding>.Ok(onboarding);
        }
        
        public async Task<BusinessResult<bool>> IsOnboarded(long userId)
        {
            Entities.Onboarding onboarding = await _repository.GetByUserId(userId);

            if (onboarding == null)
            {
                Entities.Onboarding newOnboarding = new Entities.Onboarding
                {
                    UserId = userId
                };

                await DoCreate(newOnboarding);

                return await IsOnboarded(userId);
            }
            
            return BusinessResult<bool>.Ok(onboarding.Status == OnboardingStatus.Complete);
        }

        public async Task<BusinessResult<Entities.Onboarding>> Create(Entities.Onboarding onboarding)
        {
            if (onboarding.UserId == 0)
            {
                return BusinessResult<Entities.Onboarding>.Error("User Id was not set for onboarding");
            }

            await DoCreate(onboarding);

            return BusinessResult<Entities.Onboarding>.Ok(onboarding);
        }

        private async Task DoCreate(Entities.Onboarding onboarding)
        {
            await _repository.Create(onboarding);
        }
    }
}