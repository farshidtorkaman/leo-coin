using System.Text.Json.Serialization;
using Newtonsoft.Json.Linq;

namespace Crypto.Application.PriceCalculations
{
    public class NavasanResponseVm
    {
        public NavasanResponseVm(string json)
        {
            var jObject = JObject.Parse(json);
            var jNavasan = jObject["usdt"];
            Value = (double) jNavasan["value"];
        }

        [JsonPropertyName("value")] public double Value { get; set; }
    }
}