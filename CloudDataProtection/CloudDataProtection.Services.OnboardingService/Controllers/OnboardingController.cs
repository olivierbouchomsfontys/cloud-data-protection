using System;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public OnboardingController(Lazy<OnboardingBusinessLogic> logic, IJwtDecoder jwtDecoder, IMapper mapper) : base(jwtDecoder)
        {
            _logic = logic;
            _mapper = mapper;
        }
        
        [HttpGet]
        [Route("")]
        public async Task<ActionResult> Get()
        {
            BusinessResult<Entities.Onboarding> businessResult = await _logic.Value.GetByUser(UserId);

            if (!businessResult.Success)
            {
                return NotFound(NotFoundResponse.Create("User", UserId));
            }

            return Ok(_mapper.Map<OnboardingResult>(businessResult.Data));
        }
    }
}