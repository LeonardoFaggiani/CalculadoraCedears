using CalculadoraCedears.Api.CrossCutting.Resources;
using CalculadoraCedears.Api.Domain;
using CalculadoraCedears.Api.Dto;
using CalculadoraCedears.Api.Infrastructure.Exceptions;

using CommunityToolkit.Diagnostics;

using Google.Apis.Auth;

using HtmlAgilityPack;

using System.Globalization;
using System.Net;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

namespace CalculadoraCedears.Api.Infrastructure.Repositories
{
    public interface IGoogleRepository
    {
        Task<DollarCCLQuote> TryGetCurrentDollarCCLQuoteAsync(CancellationToken cancellationToken);
        Task<GoogleFinance> TryGetFromFinanceCurrentPriceByTickerAndMarketAsync(string ticker, string market, CancellationToken cancellationToken);
        Task<GoogleJsonWebSignature.Payload?> ExchangeCodeAsync(string code);
    }

    public class GoogleRepository : IGoogleRepository
    {
        private readonly HtmlWeb HtmlWeb;
        private readonly HttpClient httpClient;
        private readonly IConfiguration configuration;

        public GoogleRepository(IConfiguration configuration)
        {
            Guard.IsNotNull(configuration, nameof(configuration));

            this.configuration = configuration;
            this.HtmlWeb = new HtmlWeb();
            this.httpClient = new HttpClient();

        }

        public async Task<DollarCCLQuote> TryGetCurrentDollarCCLQuoteAsync(CancellationToken cancellationToken)
        {
            var html = await this.httpClient.GetStringAsync($"https://www.rava.com/cotizaciones/dolares", cancellationToken);

            var match = Regex.Match(html, @":datos=""(?<json>.*?)""");

            string quoteTextJson = WebUtility.HtmlDecode(match.Groups["json"].Value);

            var rootNode = JsonNode.Parse(quoteTextJson);

            var bodyNode = rootNode?["body"];

            var quotes = bodyNode?.Deserialize<List<DollarCCLQuote>>();

            return quotes?.FirstOrDefault(x => x.DollarType != "DOLAR CCL");
        }

        public async Task<GoogleFinance> TryGetFromFinanceCurrentPriceByTickerAndMarketAsync(string ticker, string market, CancellationToken cancellationToken)
        {
            var doc = await this.HtmlWeb.LoadFromWebAsync($"https://www.google.com/finance/quote/{ticker}:{market}", cancellationToken);

            var priceDiv = doc.DocumentNode.SelectSingleNode("//div[@data-last-price]");

            if (priceDiv is null)
                throw new GoogleFinancePriceNotFoundException(Messages.PriceNotFound);

            string lastPrice = priceDiv.GetAttributeValue("data-last-price", "");

            return new GoogleFinance(Convert.ToDecimal(lastPrice, CultureInfo.InvariantCulture));
        }

        public async Task<GoogleJsonWebSignature.Payload?> ExchangeCodeAsync(string code)
        {
            var values = new Dictionary<string, string>
            {
                { "code", code },
                { "client_id", this.configuration["GoogleClient:Id"] },
                { "client_secret", this.configuration["GoogleClient:Secret"] },
                { "redirect_uri", this.configuration["GoogleClient:RedirectUri"] },
                { "grant_type", "authorization_code" }
            };

            var content = new FormUrlEncodedContent(values);

            var response = await this.httpClient.PostAsync(this.configuration["GoogleClient:AuthTokenUri"], content);

            await ThrowExceptionIsNotSuccessStatusCode(response);

            var googleAccessToken = System.Text.Json.JsonSerializer.Deserialize<GoogleAccessTokenDto>(await response.Content.ReadAsStringAsync());

            var userValidResponse = await ValidateGoogleToken(googleAccessToken.TokenId);

            return userValidResponse;
        }

        private async Task<GoogleJsonWebSignature.Payload?> ValidateGoogleToken(string googleToken)
        {
            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings()
                {
                    Audience = new[] { this.configuration["GoogleClient:Id"] }
                };

                var payload = await GoogleJsonWebSignature.ValidateAsync(googleToken, settings);

                return payload;
            }
            catch
            {
                return null;
            }
        }

        private async Task ThrowExceptionIsNotSuccessStatusCode(HttpResponseMessage responseMessage, CancellationToken cancellationToken = default)
        {
            if (!responseMessage.IsSuccessStatusCode)
            {
                var error = await responseMessage.Content.ReadAsStringAsync();
                throw new Exception($"Error getting token: {responseMessage.StatusCode} - {error}");
            }
        }
    }
}