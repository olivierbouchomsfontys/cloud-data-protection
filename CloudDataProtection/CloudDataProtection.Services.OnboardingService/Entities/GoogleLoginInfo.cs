using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace CloudDataProtection.Services.Onboarding.Entities
{
    [NotMapped]
    public class GoogleLoginInfo
    {
        public string State { get; set; }
        
        public string ClientId { get; set; }
        
        public string RedirectUri { get; set; }

        public IEnumerable<string> Scopes { get; set; }
    }
}