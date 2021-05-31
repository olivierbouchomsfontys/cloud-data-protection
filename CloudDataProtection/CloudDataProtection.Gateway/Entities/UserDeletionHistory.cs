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
        
        /// <summary>
        /// Email address of the deleted user. Will be null when user deletion is complete.
        /// </summary>
        [Encrypt]
        public string Email { get; set; }
        
        public string HashedEmail { get; set; }
        
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public DateTime? CompletedAt { get; set; }
        
        public List<UserDeletionHistoryProgress> Progress { get; set; }

        public bool IsComplete => CompletedAt.HasValue;
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