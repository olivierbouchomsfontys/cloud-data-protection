using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CloudDataProtection.Core.Entities;

namespace CloudDataProtection.Services.Onboarding.Entities
{
    [Table("Onboarding")]
    public class Onboarding : IEntity<long>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        
        public DateTime Created { get; set; } = DateTime.Now;

        public OnboardingStatus Status { get; set; } = OnboardingStatus.None;
        
        public long UserId { get; set; }
    }

    public enum OnboardingStatus
    {
        /// <summary>
        /// No steps have been completed
        /// </summary>
        None = 0,
        
        /// <summary>
        /// Account (e.g. Google Workspace) is connected
        /// </summary>
        AccountConnected = 10,
        
        /// <summary>
        /// Onboarding is completed
        /// </summary>
        Complete = 1000
    }
}