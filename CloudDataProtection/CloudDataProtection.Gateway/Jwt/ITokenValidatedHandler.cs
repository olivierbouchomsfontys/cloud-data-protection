using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace CloudDataProtection.Jwt
{
    public interface ITokenValidatedHandler
    {
        Task Handle(TokenValidatedContext context);
    }
}