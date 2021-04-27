using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CloudDataProtection.Core.Cryptography.Attributes;
using CloudDataProtection.Core.Entities;

// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace CloudDataProtection.Services.Onboarding.Entities
{
    [Table("GoogleCredential")]
    public class GoogleCredentials : IEntity<long>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        
        [Required]
        public DateTime Created { get; set; } = DateTime.Now;

        [Required]
        [Encrypt]
        public string RefreshToken { get; set; }
        
        [Required]
        public long UserId { get; set; }
    }
}