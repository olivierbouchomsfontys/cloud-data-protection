using System;

namespace CloudDataProtection.Services.MailService.Dto
{
    public class EmailChangeRequestedModel
    {
        public string NewEmail { get; set; }
        
        public string Url { get; set; }
        
        public DateTime ExpiresAt { get; set; }
    }
}