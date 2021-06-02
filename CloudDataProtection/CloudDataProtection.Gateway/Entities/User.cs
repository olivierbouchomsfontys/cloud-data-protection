using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CloudDataProtection.Core.Cryptography.Attributes;
using CloudDataProtection.Core.Entities;
using DataType = CloudDataProtection.Core.Cryptography.Attributes.DataType;

namespace CloudDataProtection.Entities
{
    [Table("User")]
    public class User : IEntity<long>
    {
        [Key]
        public long Id { get; set; }
        
        [Encrypt(DataType = DataType.EmailAddress)]
        public string Email { get; set; }
        
        public string Password { get; set; }
        
        public UserRole Role { get; set; }
    }
}