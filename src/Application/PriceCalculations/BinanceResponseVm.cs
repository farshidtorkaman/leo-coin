using System.Text.Json.Serialization;

namespace Crypto.Application.PriceCalculations
{
    public class BinanceResponseVm
    {
        [JsonPropertyName("price")]
        public double Price { get; set; }
    }
}