using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CloudDataProtection.Core.Cryptography.Attributes;
using CloudDataProtection.Core.Entities;

namespace CloudDataProtection.Services.Onboarding.Entities
{
    [Table("GoogleLoginToken")]
    public class GoogleLoginToken : IEntity<long>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        
        [Required]
        public long UserId { get; set; }

        [Required]
        [Encrypt]
        public string Token { get; set; }
        
        public DateTime? InvalidatedAt { get; set; }

        public void Invalidate()
        {
            InvalidatedAt = DateTime.Now;
        }

        public bool IsValid => !InvalidatedAt.HasValue;
    }
}