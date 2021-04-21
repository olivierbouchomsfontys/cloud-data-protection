using System;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Errors.Model;
using SendGrid.Helpers.Mail;

namespace CloudDataProtection.Services.MailService.Sender
{
    public class SendGridMailSender : IMailSender
    {
        private readonly ILogger<SendGridMailSender> _logger;
        
        private string ApiKey
        {
            get
            {
                try
                {
                    return GetEnvironmentVariable("CDP_DEV_SENDGRID");
                }
                catch (Exception e)
                {
                    _logger.LogCritical(e, "An error occurred while attempting to retrieve the API key for SendGrid");

                    throw;
                }
            }
        }

        private string SenderEmail
        {
            get
            {
                try
                {
                    return GetEnvironmentVariable("CDP_DEV_SENDGRID_SENDER");
                }
                catch (Exception e)
                {
                    _logger.LogCritical(e, "An error occurred while attempting to retrieve the sender email for SendGrid");

                    throw;
                }
            }
        }

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

        private static string GetEnvironmentVariable(string key)
        {
            string apiKey =
                Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.Process);

            if (apiKey == null)
            {
                apiKey = Environment.GetEnvironmentVariable(key);
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                // On Windows we can fallback to machine and user targets
                if (apiKey == null)
                {
                    apiKey = Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.User);
                }

                if (apiKey == null)
                {
                    apiKey = Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.Machine);
                }
            }

            if (apiKey == null)
            {
                throw new ArgumentException($"Environment variable {key} could not be retrieved");
            }

            return apiKey;
        }

    }
}