using CloudDataProtection.Core.Controllers.Data;
using Microsoft.AspNetCore.Http;

namespace CloudDataProtection.Core.Jwt
{
    public interface IJwtDecoder
    {
        int? GetUserId(IHeaderDictionary headers);

        UserRole? GetUserRole(IHeaderDictionary headers);
    }
}