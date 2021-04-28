using CloudDataProtection.Core.Environment;

namespace CloudDataProtection.Services.MailService.SendGrid.Credentials
{
    public class SendGridEnvironmentCredentialsProvider : ISendGridCredentialsProvider
    {
        public string ApiKey => EnvironmentVariableHelper.GetEnvironmentVariable("CDP_DEV_SENDGRID");
        public string SenderEmail => EnvironmentVariableHelper.GetEnvironmentVariable("CDP_DEV_SENDGRID_SENDER");
    }
}