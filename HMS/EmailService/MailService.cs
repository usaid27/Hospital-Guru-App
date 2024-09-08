using HMS.Dto;
using System.Net.Mail;
using System.Threading.Tasks;

namespace HMS.EmailService
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;

        public MailService(MailSettings mailSettings)
        {
            _mailSettings = mailSettings;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string content)
        {
            using (var message = new MailMessage(_mailSettings.FromEmail, toEmail, subject, content))
            {
                using (var client = new SmtpClient(_mailSettings.SmtpHost, _mailSettings.SmtpPort))
                {
                    client.Credentials = new System.Net.NetworkCredential(_mailSettings.SmtpUser, _mailSettings.SmtpPass);
                    client.EnableSsl = true;
                    await client.SendMailAsync(message);
                }
            }
        }
    }
}
