using System.ComponentModel.DataAnnotations;

namespace CloudDataProtection.Dto.Input
{
    public class ChangePasswordInput
    {
        [Required]
        public string CurrentPassword { get; set; }
        
        [Required]
        public string NewPassword { get; set; }
    }
}