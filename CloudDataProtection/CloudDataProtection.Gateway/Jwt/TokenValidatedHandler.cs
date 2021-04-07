using System.Threading.Tasks;
using CloudDataProtection.Business;
using CloudDataProtection.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace CloudDataProtection.Jwt
{
    public class TokenValidatedHandler : ITokenValidatedHandler
    {
        private readonly AuthenticationBusinessLogic _logic;

        public TokenValidatedHandler(AuthenticationBusinessLogic logic)
        {
            _logic = logic;
        }
        
        public async Task Handle(TokenValidatedContext context)
        {
            int userId = int.Parse(context.Principal.Identity.Name);
            
            User user = await _logic.Get(userId);

            if (user == null)
            {
                // return unauthorized if user no longer exists
                context.Fail("Unauthorized");
            }
        }
    }
}