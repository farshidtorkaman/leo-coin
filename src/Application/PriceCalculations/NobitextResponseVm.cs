using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Crypto.Application.PriceCalculations
{
    public class NobitextResponseVm
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("stats")]
        public IDictionary<string, Status> Stats { get; set; }
    }

    public class Status
    {
        [JsonPropertyName("isClosed")]
        public bool IsClosed { get; set; }

        [JsonPropertyName("bestSell")]
        public double BestSell { get; set; }

        [JsonPropertyName("bestBuy")]
        public double BestBuy { get; set; }

        [JsonPropertyName("volumeSrc")]
        public double VolumeSrc { get; set; }

        [JsonPropertyName("volumeDst")]
        public double VolumeDst { get; set; }

        [JsonPropertyName("latest")]
        public double Latest { get; set; }

        [JsonPropertyName("dayLow")]
        public double DayLow { get; set; }

        [JsonPropertyName("dayHigh")]
        public double DayHigh { get; set; }

        [JsonPropertyName("dayOpen")]
        public double DayOpen { get; set; }

        [JsonPropertyName("dayClose")]
        public double DayClose { get; set; }

        [JsonPropertyName("dayChange")]
        public double DayChange { get; set; }
    }
}
