using CloudDataProtection.Core.Environment;

namespace CloudDataProtection.Services.Onboarding.Google.Credentials
{
    public class GoogleOAuthV2EnvironmentCredentialsProvider : IGoogleOAuthV2CredentialsProvider
    {
        private const string ClientIdEnvironmentKey = "CDP_DEV_ONBOARDING_GOOGLE_OAUTH2_CLIENT_ID";
        private const string ClientSecretEnvironmentKey = "CDP_DEV_ONBOARDING_GOOGLE_OAUTH2_CLIENT_SERCET";

        public string ClientId => EnvironmentVariableHelper.GetEnvironmentVariable(ClientIdEnvironmentKey);
        public string ClientSecret => EnvironmentVariableHelper.GetEnvironmentVariable(ClientSecretEnvironmentKey);
    }
}