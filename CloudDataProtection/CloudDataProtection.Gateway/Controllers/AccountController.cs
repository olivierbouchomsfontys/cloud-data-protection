using System;
using System.Threading.Tasks;
using CloudDataProtection.Business;
using CloudDataProtection.Business.Options;
using CloudDataProtection.Core.Controllers;
using CloudDataProtection.Core.Jwt;
using CloudDataProtection.Core.Messaging;
using CloudDataProtection.Core.Rest.Errors;
using CloudDataProtection.Core.Result;
using CloudDataProtection.Dto.Input;
using CloudDataProtection.Entities;
using CloudDataProtection.Messaging.Publisher;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CloudDataProtection.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class AccountController : ServiceController
    {
        private readonly Lazy<IMessagePublisher<UserDeletedModel>> _userDeletedMessagePublisher;
        private readonly Lazy<IMessagePublisher<EmailChangeRequestedModel>> _emailChangeRequestedMessagePublisher;
        private readonly ChangeEmailOptions _changeEmailOptions;
        private readonly UserBusinessLogic _userBusinessLogic;
        private readonly AuthenticationBusinessLogic _authenticationBusinessLogic;
        
        public AccountController(IJwtDecoder jwtDecoder,
            Lazy<IMessagePublisher<UserDeletedModel>> userDeletedMessagePublisher, 
            Lazy<IMessagePublisher<EmailChangeRequestedModel>> emailChangeRequestedMessagePublisher,
            IOptions<ChangeEmailOptions> changeEmailOptions,
            UserBusinessLogic userBusinessLogic, 
            AuthenticationBusinessLogic authenticationBusinessLogic) : base(jwtDecoder)
        {
            _userDeletedMessagePublisher = userDeletedMessagePublisher;
            _userBusinessLogic = userBusinessLogic;
            _changeEmailOptions = changeEmailOptions.Value;
            _authenticationBusinessLogic = authenticationBusinessLogic;
            _emailChangeRequestedMessagePublisher = emailChangeRequestedMessagePublisher;
        }

        [HttpPatch]
        [Route("Email")]
        public async Task<ActionResult> ChangeEmail(ChangeEmailInput input)
        {
            BusinessResult<ChangeEmailRequest> changeEmailResult = await _authenticationBusinessLogic.RequestChangeEmail(UserId, input.Email);

            if (!changeEmailResult.Success)
            {
                return Conflict(ConflictResponse.Create(changeEmailResult.Message));
            }

            ChangeEmailRequest changeEmailRequest = changeEmailResult.Data;

            EmailChangeRequestedModel model = new EmailChangeRequestedModel
            {
                NewEmail = changeEmailRequest.NewEmail,
                Url = _changeEmailOptions.FormatUrl(changeEmailRequest.Token),
                ExpiresAt = changeEmailRequest.ExpiresAt
            };

            await _emailChangeRequestedMessagePublisher.Value.Send(model);
            
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