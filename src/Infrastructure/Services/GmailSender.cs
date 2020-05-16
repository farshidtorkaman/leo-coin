using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Crypto.Application.Common.Interfaces;

namespace Crypto.Infrastructure.Services
{
    public class GmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            using (var client = new SmtpClient())
            {
                var credentials = new NetworkCredential()
                {
                    UserName = "farshid2569",
                    Password = "27m4413770617"
                };

                client.Credentials = credentials;
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                client.EnableSsl = true;

                using var mail = new MailMessage()
                {
                    To = { new MailAddress(email) },
                    From = new MailAddress("farshid2569@gmail.com"),
                    Subject = subject,
                    Body = htmlMessage,
                    IsBodyHtml = true
                };

                client.Send(mail);
            }

            return Task.CompletedTask;
        }
    }
}
