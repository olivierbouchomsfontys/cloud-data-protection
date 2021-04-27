using System;
using Newtonsoft.Json;

namespace CloudDataProtection.Services.Onboarding.Google.Dto
{
    public class GoogleOAuthV2Response
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("expires_in")]
        public long ExpiresIn { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("scope")]
        public Uri Scope { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }
    }
}