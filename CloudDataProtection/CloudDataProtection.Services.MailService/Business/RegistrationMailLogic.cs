using System.Threading.Tasks;
using CloudDataProtection.Services.MailService.Business.Base;
using CloudDataProtection.Services.MailService.Sender;

namespace CloudDataProtection.Services.MailService.Business
{
    public class RegistrationMailLogic : MailLogicBase
    {
        private readonly IMailSender _sender;

        public RegistrationMailLogic(IMailSender sender)
        {
            _sender = sender;
        }

        public async Task SendUserRegistered(string email)
        {
            string subject = "Welcome to Cloud Data Protection";
            string content = @"
<p>Dear Sir / Madam,<br><br>
    Congratulations! You just completed the first step to securing all your company data. Please log in to your account and complete your registration.<br><br>
    Yours sincerely,<br><br>
    Olivier Bouchoms
  </p>";
            
            string body = ComposeBody(content);

            await _sender.Send(email, subject, body);
        }
    }
}