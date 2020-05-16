using System.Threading.Tasks;

namespace Crypto.Application.Common.Interfaces
{
    public interface IPaymentService
    {
        Task<string> Pay(double amount, string callBackUrl);

        Task<(bool result, string transactionId)> VerifyAsync(string status, string token);
    }
}