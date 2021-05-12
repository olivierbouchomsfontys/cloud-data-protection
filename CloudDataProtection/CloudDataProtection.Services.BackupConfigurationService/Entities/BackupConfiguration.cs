using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CloudDataProtection.Core.Entities;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace CloudDataProtection.Services.Subscription.Entities
{
    [Table("BackupConfiguration")]
    public class BackupConfiguration : IEntity<long>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
                
        [Required]
        public long UserId { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        [Required]
        public BackupFrequency Frequency { get; set; }
        
        public long TimeId { get; set; }
        
        [ForeignKey(nameof(TimeId))]
        public Time Time { get; set; }
    }
}