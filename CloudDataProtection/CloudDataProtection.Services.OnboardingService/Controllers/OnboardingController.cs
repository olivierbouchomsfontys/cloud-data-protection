using System;
using System.Threading.Tasks;
using AutoMapper;
using CloudDataProtection.Core.Controllers;
using CloudDataProtection.Core.Jwt;
using CloudDataProtection.Core.Messaging;
using CloudDataProtection.Core.Rest.Errors;
using CloudDataProtection.Core.Result;
using CloudDataProtection.Services.Onboarding.Business;
using CloudDataProtection.Services.Onboarding.Config;
using CloudDataProtection.Services.Onboarding.Dto;
using CloudDataProtection.Services.Onboarding.Entities;
using CloudDataProtection.Services.Onboarding.Messaging.Client.Dto;
using CloudDataProtection.Services.Onboarding.Messaging.Publisher.Dto;
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
        private readonly Lazy<IMessagePublisher<GoogleAccountConnectedModel>> _messagePublisher;
        private readonly Lazy<IRpcClient<GetUserEmailInput, GetUserEmailOutput>> _getUserEmailClient;

        public OnboardingController(
            Lazy<OnboardingBusinessLogic> logic, 
            IJwtDecoder jwtDecoder, 
            IMapper mapper, 
            IOptions<OnboardingOptions> options, 
            Lazy<IMessagePublisher<GoogleAccountConnectedModel>> messagePublisher, 
            Lazy<IRpcClient<GetUserEmailInput, GetUserEmailOutput>> getUserEmailClient) : base(jwtDecoder)
        {
            _logic = logic;
            _mapper = mapper;
            _messagePublisher = messagePublisher;
            _getUserEmailClient = getUserEmailClient;
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
        [Route("[controller]/GoogleLogin")]
        public async Task<ActionResult> GoogleLogin()
        {
            string code = Request.Query["code"];
            string token = Request.Query["state"];
            
            BusinessResult<GoogleCredentials> businessResult = await _logic.Value.CreateCredentials(code, token);

            if (!businessResult.Success)
            {
                return Problem(businessResult.Message);
            }

            long userId = businessResult.Data.UserId;

            GetUserEmailInput input = new GetUserEmailInput(userId);

            GetUserEmailOutput response = await _getUserEmailClient.Value.Request(input);

            GoogleAccountConnectedModel model = new GoogleAccountConnectedModel(userId, response.Email);

            await _messagePublisher.Value.Send(model);
            
            return Redirect(_options.RedirectUri);
        }
    }
}