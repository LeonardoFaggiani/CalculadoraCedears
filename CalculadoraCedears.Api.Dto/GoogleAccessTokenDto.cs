using System.Text.Json.Serialization;

namespace CalculadoraCedears.Api.Dto
{
    public class GoogleAccessTokenDto
    {
        [JsonPropertyName("id_token")]
        public string TokenId { get; set; }
    }
}
