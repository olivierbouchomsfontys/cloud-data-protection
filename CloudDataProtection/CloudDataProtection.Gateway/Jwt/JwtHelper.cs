using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CloudDataProtection.Core.Jwt;
using CloudDataProtection.Entities;
using Microsoft.IdentityModel.Tokens;

namespace CloudDataProtection.Jwt
{
    public class JwtHelper : IJwtHelper
    {
        private const int ExpirationTimeDays = 14;
        
        public string GenerateToken(User user)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            
            // TODO Use Azure Key Vault
            byte[] key = Encoding.ASCII.GetBytes("jwtSecretButNowLonger");

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(CustomClaimTypes.UserRole, ((int) user.Role).ToString()),
                }),
                Expires = DateTime.UtcNow.AddDays(ExpirationTimeDays),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            
            string tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }
    }
}