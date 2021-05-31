using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CloudDataProtection.Core.Cryptography.Attributes;

namespace CloudDataProtection.Entities
{
    [Table("UserDeletionHistory")]
    public class UserDeletionHistory
    {
        [Key]
        public long Id { get; set; }
        
        [Required]
        public long UserId { get; set; }
        
        [Encrypt]
        public string Email { get; set; }
        
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public DateTime? CompletedAt { get; set; }
        
        public List<UserDeletionHistoryProgress> Progress { get; set; }
    }

    [Table("UserDeletionHistoryProgress")]
    public class UserDeletionHistoryProgress
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        
        [Required]
        public string ServiceName { get; set; }
        
        public DateTime? StartedAt { get; set; }
        
        public DateTime? CompletedAt { get; set; }
        
        public long UserDeletionHistoryId { get; set; }
        
        [ForeignKey(nameof(UserDeletionHistoryId))]
        public UserDeletionHistory History { get; set; }
    }
}