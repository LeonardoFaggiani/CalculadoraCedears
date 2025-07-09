using System.Text.Json.Serialization;

namespace CalculadoraCedears.Api.Domain
{
    public class DollarCCLQuote
    {
        [JsonPropertyName("fecha")]
        public string Date { get; set; }
        [JsonPropertyName("ultimo")]
        public decimal? PriceCCL { get; set; }
        [JsonPropertyName("variacion")]
        public decimal VariationCCL { get; set; }
        [JsonPropertyName("especie")]
        public string DollarType { get; set; }
    }
}
