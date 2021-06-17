using System;
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

        public async Task SendUserDeletionComplete(string email)
        {
            string subject = "Account deletion complete";
            string content = @"
<p>Dear Sir / Madam,<br><br>
    Hereby we confirm the deletion of your account has been completed. All your data has been deleted. We thank you for your trust and hope to see you again in the future.<br><br>
    Yours sincerely,<br><br>
    Olivier Bouchoms
  </p>";

            string body = ComposeBody(content);
            
            await _sender.Send(email, subject, body);
        }

        public async Task SendEmailChangeRequested(string email, string url, DateTime expiration)
        {
            string subject = "Confirm email change";
            string content = $@"
<p>Dear Sir / Madam,<br><br>
    We received a request to change your password. Please click <a href='{url}'>here</a> to confirm this change.<br><br>
    If the link above doesn't work, please copy and paste the following link in your web browser: {url}<br><br>
    This link will expire at {expiration.ToString("F")}<br><br>
    If you did not perform this action, please contact us by replying to this email.<br><br>
    Yours sincerely,<br><br>
    Olivier Bouchoms
  </p>";

            string body = ComposeBody(content);
            
            await _sender.Send(email, subject, body);
        }
    }
}