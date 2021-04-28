using System.Threading.Tasks;
using CloudDataProtection.Services.MailService.Sender;

namespace CloudDataProtection.Services.MailService.Business
{
    public class RegistrationMailLogic
    {
        private readonly IMailSender _sender;

        private const string Template = @"
<div style='width: max(40%, 480px); font-family:sans-serif;'>
  <div style='background-color: #3f51b5; display: block; font-family: sans-serif; color: #fff; padding: 1rem; box-shadow: 0px 2px 4px -1px rgba(0,0,0,0.2),0px 4px 5px 0px rgba(0,0,0,0.14),0px 1px 10px 0px rgba(0,0,0,0.12)'>Cloud Data Protection</div>
  {0}
</div>";

        public RegistrationMailLogic(IMailSender sender)
        {
            _sender = sender;
        }

        public async Task SendUserRegistered(string email)
        {
            string subject = "Welcome to Cloud Data Protection";
            string content = @"
<p>Dear Sir / Madam,</br></br>
    Congratulations! You just completed the first step to securing all your company data. Please log in to your account and complete your registration.</br></br>
    Yours sincerely,</br></br>
    Olivier Bouchoms
  </p>";
            
            string body = string.Format(Template, content);

            await _sender.Send(email, subject, body);
        }
    }
}