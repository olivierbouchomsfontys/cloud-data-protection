using System;
using CloudDataProtection.Services.Onboarding.Entities;

namespace CloudDataProtection.Services.Onboarding.Dto
{
    public class OnboardingResult
    {
        public long Id { get; set; }
        
        public DateTime Created { get; set; }

        public OnboardingStatus Status { get; set; }
        
        public long UserId { get; set; }
        
        public GoogleLoginInfoResult LoginInfo { get; set; }
    }
}