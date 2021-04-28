using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CloudDataProtection.Services.MailService.Sender;
using CloudDataProtection.Services.MailService.SendGrid.Credentials;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace CloudDataProtection.Services.MailService.SendGrid
{
    public class SendGridMailSender : IMailSender
    {
        private readonly ISendGridCredentialsProvider _credentialsProvider;
        private readonly ILogger<SendGridMailSender> _logger;
        
        public SendGridMailSender(ISendGridCredentialsProvider credentialsProvider, ILogger<SendGridMailSender> logger)
        {
            _credentialsProvider = credentialsProvider;
            _logger = logger;
        }
        
        public async Task Send(string recipient, string subject, string body)
        {
            SendGridClient client = new SendGridClient(_credentialsProvider.ApiKey);

            SendGridMessage message = Compose(recipient, subject, body, _credentialsProvider.SenderEmail);

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
                subject, StripHtml(body), body);
        }

        private static string StripHtml(string input)
        {
            return Regex.Replace(input, "<.*?>", string.Empty);
        }
    }
}