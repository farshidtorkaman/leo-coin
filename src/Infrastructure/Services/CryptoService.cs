using System.Linq;
using Crypto.Application.Common.Interfaces;
using Crypto.Application.PriceCalculations;
using Newtonsoft.Json;

namespace Crypto.Infrastructure.Services
{
    public class CryptoService : ICryptoService
    {
        private readonly string NobitexBaseUrl = "https://api.nobitex.ir/market/stats";
        //private readonly string BinanceBaseUrl = "https://api.binance.com/api/v3/ticker/price?symbol=";

        public double ConvertToToman(double amount, string symbol)
        {
            return GetTomanPriceFromNobitex(amount, symbol);
        }

        public double GetWage(double amount, string symbol)
        {
            return GetWageFromNobitext(amount, symbol);
        }

        //private async Task<double> GetTeterDollar(double amount, string symbol)
        //{
        //    using var httpClient = new HttpClient();
        //    using var response = await httpClient.GetAsync(BinanceBaseUrl + symbol);
        //    var apiResponse = await response.Content.ReadAsStringAsync();
        //    var binanceResponse = JsonConvert.DeserializeObject<BinanceResponseVm>(apiResponse);

        //    var some = binanceResponse.Price * amount;
        //    return some;
        //}

        private double GetTomanPriceFromNobitex(double amount, string symbol)
        {
            var webRequest = new MyWebRequest(NobitexBaseUrl, "POST", "srcCurrency=" + symbol + "&dstCurrency=rls");
            var response = webRequest.GetResponse();
            var nobitextResponse = JsonConvert.DeserializeObject<NobitextResponseVm>(response);

            var nobitext = nobitextResponse.Stats.Values.First();
            if (nobitext != null)
            {
                return amount * nobitext.Latest;
            }

            return 0;
            //using var httpClient = new HttpClient();
            //using var response = await httpClient.PostAsync(NobitexBaseUrl, );
            //var apiResponse = await response.Content.ReadAsStringAsync();

            //var navasanResponse = new NavasanResponseVm(apiResponse);

            //return navasanResponse.Value * amount;
        }

        private double GetWageFromNobitext(double amount, string symbol)
        {
            var webRequest = new MyWebRequest(NobitexBaseUrl, "POST", "srcCurrency=" + symbol + "&dstCurrency=rls");
            var response = webRequest.GetResponse();
            var nobitextResponse = JsonConvert.DeserializeObject<NobitextResponseVm>(response);

            var nobitext = nobitextResponse.Stats.Values.First();
            if (nobitext != null)
            {
                return amount * nobitext.BestSell - amount * nobitext.BestBuy;
            }

            return 0;
        }
    }
}