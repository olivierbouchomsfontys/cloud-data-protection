// ReSharper disable UnusedAutoPropertyAccessor.Global

using System.Collections.Generic;

namespace CloudDataProtection.Services.Onboarding.Google.Options
{
    public class GoogleOAuthV2Options
    {
        public string Endpoint { get; set; }

        public string RedirectUri { get; set; }
        
        public string GrantType { get; set; }
        
        public IEnumerable<string> Scopes { get; set; }
    }
}