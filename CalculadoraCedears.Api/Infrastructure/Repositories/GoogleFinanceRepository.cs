using CalculadoraCedears.Api.CrossCutting.Resources;
using CalculadoraCedears.Api.Domain;
using CalculadoraCedears.Api.Infrastructure.Exceptions;

using HtmlAgilityPack;

using System.Globalization;

namespace CalculadoraCedears.Api.Infrastructure.Repositories
{
    public interface IGoogleFinanceRepository
    {
        Task<GoogleFinance> TryGetCurrentPriceByTickerAndMarketAsync(string ticker, string market, CancellationToken cancellationToken);
    }

    public class GoogleFinanceRepository : IGoogleFinanceRepository
    {
        private readonly HtmlWeb HtmlWeb;

        public GoogleFinanceRepository()
        {
            HtmlWeb = new HtmlWeb();
        }

        public async Task<GoogleFinance> TryGetCurrentPriceByTickerAndMarketAsync(string ticker, string market, CancellationToken cancellationToken)
        {
            var doc = await this.HtmlWeb.LoadFromWebAsync($"https://www.google.com/finance/quote/{ticker}:{market}", cancellationToken);

            var priceDiv = doc.DocumentNode.SelectSingleNode("//div[@data-last-price]");

            if (priceDiv is null)
                throw new GoogleFinancePriceNotFoundException(Messages.PriceNotFound);

            string lastPrice = priceDiv.GetAttributeValue("data-last-price", "");        

            return new GoogleFinance(Convert.ToDecimal(lastPrice, CultureInfo.InvariantCulture));
        }     
    }
}