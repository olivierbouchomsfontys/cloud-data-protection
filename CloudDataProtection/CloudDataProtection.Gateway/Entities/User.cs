using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloudDataProtection.Entities
{
    [Table("User")]
    public class User
    {
        [Key]
        public long Id { get; set; }
        
        public string Email { get; set; }
        
        public string Password { get; set; }
    }
}