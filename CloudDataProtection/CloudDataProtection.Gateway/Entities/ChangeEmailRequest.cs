using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CloudDataProtection.Core.Cryptography.Attributes;

namespace CloudDataProtection.Entities
{
    [Table("ChangeEmailRequest")]
    public class ChangeEmailRequest
    {
        [Key]
        public long Id { get; set; }
        
        [Required]
        public long UserId { get; set; }
        
        [Required]
        [Encrypt]
        public string NewEmail { get; set; }
        
        [Required]
        [Encrypt]
        public string Token { get; set; }
        
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public DateTime? ConfirmedAt { get; set; }
        
        /// <summary>
        /// A request is invalidated if another email change is requested
        /// </summary>
        public DateTime? InvalidatedAt { get; set; }

        public DateTime ExpiresAt { get; set; }

        public bool IsValid => ConfirmedAt == null && InvalidatedAt == null && ExpiresAt > DateTime.Now;

        public void Invalidate()
        {
            InvalidatedAt??= DateTime.Now;
        }
    }
}