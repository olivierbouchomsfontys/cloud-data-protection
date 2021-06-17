namespace CloudDataProtection.Business.Options
{
    public class ChangeEmailOptions
    {
        public int ExpiresInMinutes { get; set; }
        public string Url { get; set; }

        public string FormatUrl(string token) => string.Format(Url, token);
    }
}