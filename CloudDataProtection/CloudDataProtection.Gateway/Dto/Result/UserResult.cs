using CloudDataProtection.Entities;

namespace CloudDataProtection.Dto.Result
{
    public class UserResult
    {
        public long Id { get; set; }
        
        public string Email { get; set; }
        
        public UserRole Role { get; set; }
    }
}