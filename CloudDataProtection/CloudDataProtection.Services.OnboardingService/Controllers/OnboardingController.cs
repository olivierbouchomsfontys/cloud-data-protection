﻿using System;
using System.Threading.Tasks;
using AutoMapper;
using CloudDataProtection.Core.Controllers;
using CloudDataProtection.Core.Jwt;
using CloudDataProtection.Core.Rest.Errors;
using CloudDataProtection.Core.Result;
using CloudDataProtection.Services.Onboarding.Business;
using CloudDataProtection.Services.Onboarding.Config;
using CloudDataProtection.Services.Onboarding.Dto;
using CloudDataProtection.Services.Onboarding.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CloudDataProtection.Services.Onboarding.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OnboardingController : ServiceController
    {
        private readonly Lazy<OnboardingBusinessLogic> _logic;
        private readonly IMapper _mapper;
        private readonly OnboardingOptions _options;

        public OnboardingController(Lazy<OnboardingBusinessLogic> logic, IJwtDecoder jwtDecoder, IMapper mapper, IOptions<OnboardingOptions> options) : base(jwtDecoder)
        {
            _logic = logic;
            _mapper = mapper;
            _options = options.Value;
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

            OnboardingResult result = _mapper.Map<OnboardingResult>(businessResult.Data);

            if (businessResult.Data.Status == OnboardingStatus.None)
            {
                BusinessResult<GoogleLoginInfo> loginInfoBusinessResult = await _logic.Value.GetLoginInfo(UserId);

                if (!loginInfoBusinessResult.Success)
                {
                    return Problem(loginInfoBusinessResult.Message);
                }
                
                result.LoginInfo = _mapper.Map<GoogleLoginInfoResult>(loginInfoBusinessResult.Data);
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("GoogleLogin")]
        public async Task<ActionResult> GoogleLogin()
        {
            string code = Request.Query["code"];
            string token = Request.Query["state"];
            
            BusinessResult<GoogleCredentials> businessResult = await _logic.Value.CreateCredentials(code, token);

            if (!businessResult.Success)
            {
                return Problem(businessResult.Message);
            }
            
            return Redirect(_options.RedirectUri);
        }
    }
}