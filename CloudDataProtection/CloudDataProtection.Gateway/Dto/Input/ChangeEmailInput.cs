using System.ComponentModel.DataAnnotations;

namespace CloudDataProtection.Dto.Input
{
    public class ChangeEmailInput
    {
        [Required]
        public string Email { get; set; }
    }
}