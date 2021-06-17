using System;
using System.Threading.Tasks;
using CloudDataProtection.Business;
using CloudDataProtection.Core.Messaging;
using CloudDataProtection.Core.Rest.Errors;
using CloudDataProtection.Core.Result;
using CloudDataProtection.Dto.Input;
using CloudDataProtection.Dto.Result;
using CloudDataProtection.Entities;
using CloudDataProtection.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CloudDataProtection.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly AuthenticationBusinessLogic _logic;
        private readonly IJwtHelper _jwtHelper;
        private readonly Lazy<IMessagePublisher<UserResult>> _messagePublisher;

        public AuthenticationController(AuthenticationBusinessLogic logic, IJwtHelper jwtHelper, 
            Lazy<IMessagePublisher<UserResult>> messagePublisher)
        {
            _logic = logic;
            _jwtHelper = jwtHelper;
            _messagePublisher = messagePublisher;
        }
        
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<ActionResult> Authenticate([FromBody] LoginInput input)
        {
            BusinessResult<User> businessResult = await _logic.Authenticate(input.Email, input.Password);

            if (!businessResult.Success)
            {
                return Unauthorized(UnauthorizedResponse.Create());
            }

            User user = businessResult.Data;

            AuthenticateResult result = new AuthenticateResult
            {
                User = new UserResult
                {
                    Email = user.Email,
                    Id = user.Id,
                    Role = user.Role
                },
                Token = _jwtHelper.GenerateToken(user)
            };
            
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterInput model)
        {
            User user = new User()
            {
                Email = model.Email,
                Role = UserRole.Client
            };

            // create user
            BusinessResult<User> businessResult = await _logic.Create(user, model.Password);

            if (businessResult.Success)
            {
                UserResult result = new UserResult
                {
                    Email = user.Email,
                    Id = user.Id,
                    Role = user.Role
                };

                await _messagePublisher.Value.Send(result);
                
                return Ok(result);
            }
            else
            {
                return Conflict(businessResult.Message);
            }
        }
    }
}