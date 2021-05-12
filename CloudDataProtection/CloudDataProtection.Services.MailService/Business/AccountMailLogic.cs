using System.Threading.Tasks;
using CloudDataProtection.Services.MailService.Business.Base;
using CloudDataProtection.Services.MailService.Sender;

namespace CloudDataProtection.Services.MailService.Business
{
    public class AccountMailLogic : MailLogicBase
    {
        private readonly IMailSender _sender;

        public AccountMailLogic(IMailSender sender)
        {
            _sender = sender;
        }

        public async Task SendAccountConnected(string email)
        {
            string subject = "Google account connected";
            string content = @"
<p>Dear Sir / Madam,<br><br>
    Congratulations! You just connected your Google account to Cloud Data Protection. If you did not perform this action, please contact us by replying to this email.<br><br>
    Yours sincerely,<br><br>
    Olivier Bouchoms
  </p>";
            
            string body = ComposeBody(content);

            await _sender.Send(email, subject, body);
        }
    }
}