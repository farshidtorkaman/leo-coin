using System.Threading.Tasks;

namespace Crypto.Application.Common.Interfaces
{
    public interface ICryptoService
    {
        Task<double> ConvertToToman(double amount, string symbol);
    }
}