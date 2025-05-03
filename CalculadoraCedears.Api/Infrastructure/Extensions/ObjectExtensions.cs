using AutoMapper;

using CalculadoraCedears.Api.Dto;

namespace CalculadoraCedears.Api.Infrastructure.Extensions
{
    public static class ObjectExtensions
    {
        public static List<CedearWithStockHoldingDto> ConvertToResult(this Dictionary<string, List<Domain.CedearsStockHolding>> keyValuePairs, IMapper mapper)
        {
            var cedearWithStockHoldings = new List<CedearWithStockHoldingDto>();

            foreach (var ticker in keyValuePairs.Keys)
            {
                var cedear = keyValuePairs[ticker].First().Cedear;

                var cedearWithStockHolding = new CedearWithStockHoldingDto
                {
                    Ticker = ticker,
                    Ratio = cedear.Ratio,
                    Id = cedear.Id,
                    Name = cedear.Name,
                    Price = cedear.Price.GetValueOrDefault(0)
                };

                foreach (var cedearsStockHolding in keyValuePairs[ticker])
                {
                    var cedearsStockHoldingDto = mapper.Map<Domain.CedearsStockHolding, CedearStockHoldingDto>(cedearsStockHolding);
                    cedearsStockHoldingDto.CurrentPriceUsd = (decimal)cedear.Price;
                    cedearWithStockHolding.CedearsStockHoldings.Add(cedearsStockHoldingDto);
                }


                cedearWithStockHoldings.Add(cedearWithStockHolding);
            }

            return cedearWithStockHoldings;
        }
    }
}
