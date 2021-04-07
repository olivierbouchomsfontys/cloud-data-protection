using System.Threading.Tasks;
using CloudDataProtection.Services.MailService.Sender;

namespace CloudDataProtection.Services.MailService.Business
{
    public class RegistrationMailLogic
    {
        private readonly IMailSender _sender;

        public RegistrationMailLogic(IMailSender sender)
        {
            _sender = sender;
        }

        public async Task SendUserRegistered(string email)
        {
            string subject = "Welcome to Cloud Data Protection";
            string body = @"Dear Sir / Madam,

Congratulations! You just completed the first step to securing all your company data. Please log in to your account and complete your registration.

Yours sincerely,

Olivier Bouchoms
";

            await _sender.Send(email, subject, body);
        }
    }
}