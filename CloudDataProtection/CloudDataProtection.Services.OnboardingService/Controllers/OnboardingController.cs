using System;
using System.Threading.Tasks;
using CloudDataProtection.Core.Rest.Errors;
using CloudDataProtection.Services.Onboarding.Business;
using CloudDataProtection.Services.Onboarding.Dto;
using Microsoft.AspNetCore.Mvc;

namespace CloudDataProtection.Services.Onboarding.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OnboardingController : ControllerBase
    {
        private readonly Lazy<OnboardingBusinessLogic> _logic;

        public OnboardingController(Lazy<OnboardingBusinessLogic> logic)
        {
            _logic = logic;
        }
        
        [HttpGet]
        public async Task<ActionResult> IsComplete(int userId)
        {
            var businessResult = await _logic.Value.IsOnboarded(userId);

            if (!businessResult.Success)
            {
                return NotFound(NotFoundResponse.Create("User", userId));
            }

            IsCompleteResult result = new IsCompleteResult
            {
                IsComplete = businessResult.Data
            };
            
            return Ok(result);
        }
    }
}