using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CloudDataProtection.Core.Entities;

namespace CloudDataProtection.Entities
{
    [Table("User")]
    public class User : IEntity<long>
    {
        [Key]
        public long Id { get; set; }
        
        public string Email { get; set; }
        
        public string Password { get; set; }
        
        public UserRole Role { get; set; }
    }
}