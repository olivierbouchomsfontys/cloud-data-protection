using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CloudDataProtection.Core.Jwt;
using CloudDataProtection.Core.Jwt.Options;
using CloudDataProtection.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CloudDataProtection.Jwt
{
    public class JwtHelper : IJwtHelper
    {
        private const int ExpirationTimeDays = 14;

        private readonly JwtSecretOptions _options;
        
        public JwtHelper(IOptions<JwtSecretOptions> options)
        {
            _options = options.Value;
        }
        
        public string GenerateToken(User user)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(CustomClaimTypes.UserRole, ((int) user.Role).ToString()),
                }),
                Expires = DateTime.UtcNow.AddDays(ExpirationTimeDays),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_options.Key), SecurityAlgorithms.HmacSha256Signature)
            };
            
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            
            string tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }
    }
}