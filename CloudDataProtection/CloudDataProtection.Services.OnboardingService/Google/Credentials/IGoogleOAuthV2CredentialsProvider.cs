namespace CloudDataProtection.Services.Onboarding.Google.Credentials
{
    public interface IGoogleOAuthV2CredentialsProvider
    {
        string ClientId { get; }
        string ClientSecret { get; }
    }
}