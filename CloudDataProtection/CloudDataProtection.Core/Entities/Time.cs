using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloudDataProtection.Core.Entities
{
    [Table("Time")]
    public class Time : IEntity<long>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        
        [Range(0, 23)]
        public int Hours { get; set; }
        
        [Range(0, 59)]
        public int Minutes { get; set; }
        
        [Range(0, 59)]
        public int Seconds { get; set; }

        public Time() { }

        public Time(int hours, int minutes)
        {
            Hours = hours;
            Minutes = minutes;
        }
        
        public Time(int hours, int minutes, int seconds)
        {
            Hours = hours;
            Minutes = minutes;
            Seconds = seconds;
        }
    }
}