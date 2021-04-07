using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CloudDataProtection.Business;
using CloudDataProtection.Dto;
using CloudDataProtection.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CloudDataProtection.Controllers
{
    public class AuthenticationController : ControllerBase
    {
        private readonly AuthenticationBusinessLogic _authenticationBusinessLogic;

        public AuthenticationController(AuthenticationBusinessLogic authenticationBusinessLogic)
        {
            _authenticationBusinessLogic = authenticationBusinessLogic;
        }
        
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<ActionResult> Authenticate([FromBody] LoginInput input)
        {
            var user = await _authenticationBusinessLogic.Authenticate(input.Username, input.Password);

            if (user == null)
                return Unauthorized(new { message = "Username or password is incorrect" });

            var tokenHandler = new JwtSecurityTokenHandler();
            
            // TODO Replace
            var key = Encoding.ASCII.GetBytes("jwtSecretButNowLonger");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // return basic user info and authentication token
            return Ok(new AuthenticateResult
            {
                User = new UserResult
                {
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Id = user.Id
                },
                Token = tokenString
            });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterInput model)
        {
            // map model to entity
            var user = new User()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email
            };

            // create user
            await _authenticationBusinessLogic.Create(user, model.Password);
            return Ok();
        }
    }
}