using System.Threading.Tasks;
using CloudDataProtection.Core.Environment;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace CloudDataProtection.Services.MailService.Sender
{
    public class SendGridMailSender : IMailSender
    {
        private readonly ILogger<SendGridMailSender> _logger;
        
        private string ApiKey => EnvironmentVariableHelper.GetEnvironmentVariable("CDP_DEV_SENDGRID");

        private string SenderEmail => EnvironmentVariableHelper.GetEnvironmentVariable("CDP_DEV_SENDGRID_SENDER");

        public SendGridMailSender(ILogger<SendGridMailSender> logger)
        {
            _logger = logger;
        }
        
        public async Task Send(string recipient, string subject, string body)
        {
            SendGridClient client = new SendGridClient(ApiKey);

            SendGridMessage message = Compose(recipient, subject, body, SenderEmail);

            Response response = await client.SendEmailAsync(message);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"An error occured while sending a request to SendGrid: {response.StatusCode}, {await response.Body.ReadAsStringAsync()}");
            }
        }

        private static SendGridMessage Compose(string recipient, string subject, string body, string sender)
        {
            return MailHelper.CreateSingleEmail(
                new EmailAddress(sender, "Cloud Data Protection (development)"), 
                new EmailAddress(recipient),
                subject, body, body);
        }
    }
}