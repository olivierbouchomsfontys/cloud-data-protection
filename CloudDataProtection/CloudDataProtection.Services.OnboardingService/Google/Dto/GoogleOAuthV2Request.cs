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

        public GoogleOAuthV2Request HidePII()
        {
            return new GoogleOAuthV2Request
            {
                code = $"HIDDEN ({this.code?.Length} chars)",
                client_id = this.client_id,
                client_secret = $"HIDDEN ({this.client_secret?.Length} chars)",
                redirect_uri = this.redirect_uri,
                grant_type = this.grant_type
            };
        }
    }
}