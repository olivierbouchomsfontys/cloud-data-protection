using CloudDataProtection.Entities;

namespace CloudDataProtection.Jwt
{
    public interface IJwtHelper
    {
        string GenerateToken(User user);
    }
}