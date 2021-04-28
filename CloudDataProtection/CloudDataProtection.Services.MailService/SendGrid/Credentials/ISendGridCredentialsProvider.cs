namespace CloudDataProtection.Services.MailService.SendGrid.Credentials
{
    public interface ISendGridCredentialsProvider
    {
        string ApiKey { get; }
        string SenderEmail { get; }
    }
}