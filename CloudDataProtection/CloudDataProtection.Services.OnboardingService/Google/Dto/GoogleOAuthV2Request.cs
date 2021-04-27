// ReSharper disable InconsistentNaming
// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace CloudDataProtection.Services.Onboarding.Google.Dto
{
    public class GoogleOAuthV2Request
    {
        public string code { get; set; }
        
        public string client_id { get; set; }
        
        public string client_secret { get; set; }
        
        public string redirect_uri { get; set; }
        
        public string grant_type { get; set; }
    }
}