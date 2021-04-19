using System;
using System.Threading.Tasks;
using CloudDataProtection.Core.Controllers;
using CloudDataProtection.Core.Jwt;
using CloudDataProtection.Core.Rest.Errors;
using CloudDataProtection.Core.Result;
using CloudDataProtection.Services.Onboarding.Business;
using CloudDataProtection.Services.Onboarding.Dto;
using Microsoft.AspNetCore.Mvc;

namespace CloudDataProtection.Services.Onboarding.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OnboardingController : ServiceController
    {
        private readonly Lazy<OnboardingBusinessLogic> _logic;

        public OnboardingController(Lazy<OnboardingBusinessLogic> logic, IJwtDecoder jwtDecoder) : base(jwtDecoder)
        {
            _logic = logic;
        }
        
        [HttpGet]
        [Route("")]
        public async Task<ActionResult> Get()
        {
            BusinessResult<bool> businessResult = await _logic.Value.IsOnboarded(UserId);

            if (!businessResult.Success)
            {
                return NotFound(NotFoundResponse.Create("User", UserId));
            }

            IsCompleteResult result = new IsCompleteResult
            {
                IsComplete = businessResult.Data
            };
            
            return Ok(result);
        }
    }
}