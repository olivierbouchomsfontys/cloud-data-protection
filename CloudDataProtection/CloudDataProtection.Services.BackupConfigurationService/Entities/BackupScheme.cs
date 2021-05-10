using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CloudDataProtection.Core.Entities;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace CloudDataProtection.Services.Subscription.Entities
{
    [Table("BackupScheme")]
    public class BackupScheme : IEntity<long>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        [Required]
        public BackupFrequency Frequency { get; set; }
        
        public long TimeId { get; set; }
        
        [ForeignKey(nameof(TimeId))]
        public virtual Time Time { get; set; }
    }

    public enum BackupFrequency
    {
        Daily = 1
    }
}