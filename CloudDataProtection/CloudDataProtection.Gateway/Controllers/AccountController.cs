using System;
using System.Threading.Tasks;
using CloudDataProtection.Business;
using CloudDataProtection.Core.Controllers;
using CloudDataProtection.Core.Jwt;
using CloudDataProtection.Core.Messaging;
using CloudDataProtection.Core.Result;
using CloudDataProtection.Dto;
using CloudDataProtection.Dto.Input;
using CloudDataProtection.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CloudDataProtection.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class AccountController : ServiceController
    {
        private readonly Lazy<IMessagePublisher<UserDeletedModel>> _userDeletedMessagePublisher;
        private readonly UserBusinessLogic _userBusinessLogic;
        
        public AccountController(IJwtDecoder jwtDecoder, Lazy<IMessagePublisher<UserDeletedModel>> userDeletedMessagePublisher, UserBusinessLogic userBusinessLogic) : base(jwtDecoder)
        {
            _userDeletedMessagePublisher = userDeletedMessagePublisher;
            _userBusinessLogic = userBusinessLogic;
        }

        [HttpPatch]
        [Route("Email")]
        public async Task<ActionResult> ChangeEmail(ChangeEmailInput input)
        {
            return Ok();
        }

        [HttpDelete]
        [Route("")]
        public async Task<ActionResult> Delete()
        {
            BusinessResult<User> getUserResult = await _userBusinessLogic.Get(UserId);

            if (!getUserResult.Success || getUserResult.Data == null)
            {
                return Forbid();
            }

            User user = getUserResult.Data;

            UserDeletedModel model = new UserDeletedModel
            {
                Email = user.Email,
                UserId = user.Id
            };

            await _userBusinessLogic.Delete(user.Id);

            await _userDeletedMessagePublisher.Value.Send(model);
            
            return Accepted();
        }
    }
}