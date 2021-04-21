using System.Threading.Tasks;

namespace CloudDataProtection.Services.MailService.Sender
{
    public interface IMailSender
    {
        Task Send(string recipient, string subject, string body);
    }
}