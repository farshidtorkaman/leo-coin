using System.Threading.Tasks;

namespace Crypto.Application.Common.Interfaces
{
    public interface ICryptoService
    {
        double ConvertToToman(double amount, string symbol);
        double GetWage(double amount, string symbol);
    }
}