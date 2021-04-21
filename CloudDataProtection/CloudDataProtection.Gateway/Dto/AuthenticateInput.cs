using System.ComponentModel.DataAnnotations;

namespace CloudDataProtection.Dto
{
    public class LoginInput
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}