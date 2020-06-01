using System.Threading.Tasks;

namespace Crypto.Application.Common.Interfaces
{
    public interface IEmailSender
    {
        void SendEmailAsync(string email, string subject, string htmlMessage);
    }
}
