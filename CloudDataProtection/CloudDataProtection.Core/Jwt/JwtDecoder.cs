using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace CloudDataProtection.Core.Jwt
{
    public class JwtDecoder : IJwtDecoder
    {
        private const string AuthenticationScheme = "Bearer";
        private const string DecryptionKey = "jwtSecretButNowLonger";
        
        public int? GetUserId(IHeaderDictionary headers)
        {
            int? userId = headers
                .Where(header => header.Key == "Authorization")
                .Select(header => header.Value)
                .Select(value => value.ToString())
                .Select(GetToken)
                .Select(GetClaims)
                .Select(GetUserId)
                .FirstOrDefault();

            return userId;
        }

        private string GetToken(string header)
        {
            if (!header.StartsWith(AuthenticationScheme))
            {
                return null;
            }

            return header.Replace(AuthenticationScheme, string.Empty).Trim();
        }

        private IEnumerable<Claim> GetClaims(string token)
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            SecurityToken jsonToken = handler.ReadToken(token);
            JwtSecurityToken tokenS = jsonToken as JwtSecurityToken;

            if (tokenS == null)
            {
                return null;
            }

            return tokenS.Claims;
        }

        private int? GetUserId(IEnumerable<Claim> claims)
        {
            return claims
                .Where(c => c.Type == "unique_name")
                .Select(claim => claim.Value)
                .Select(value => int.TryParse(value, out int id) ? id : null as int?)
                .FirstOrDefault();
        }
    }
}