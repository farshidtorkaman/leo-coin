using System.Net.Http;
using System.Threading.Tasks;
using Crypto.Application.Common.Interfaces;
using Crypto.Application.PriceCalculations;
using Newtonsoft.Json;

namespace Crypto.Infrastructure.Services
{
    public class CryptoService : ICryptoService
    {
        private readonly string NavasanBaseUrl = "http://api.navasan.tech/latest/?api_key=d4WCE8VOXfJRrsRjtwFXKcP79dzKl6uR&item=usdt";
        private readonly string BinanceBaseUrl = "https://api.binance.com/api/v3/ticker/price?symbol=";

        public async Task<double> ConvertToToman(double amount, string symbol)
        {
            var teterDollar = await GetTeterDollar(amount, symbol);
            return await GetTomanPriceFromNavasan(teterDollar);
        }

        private async Task<double> GetTeterDollar(double amount, string symbol)
        {
            using var httpClient = new HttpClient();
            using var response = await httpClient.GetAsync(BinanceBaseUrl + symbol);
            var apiResponse = await response.Content.ReadAsStringAsync();
            var binanceResponse = JsonConvert.DeserializeObject<BinanceResponseVm>(apiResponse);

            var some = binanceResponse.Price * amount;
            return some;
        }

        private async Task<double> GetTomanPriceFromNavasan(double amount)
        {
            using var httpClient = new HttpClient();
            using var response = await httpClient.GetAsync(NavasanBaseUrl);
            var apiResponse = await response.Content.ReadAsStringAsync();
            
            var navasanResponse = new NavasanResponseVm(apiResponse);

            return navasanResponse.Value * amount;
        }
    }
}