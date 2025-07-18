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
                var broker = keyValuePairs[ticker].First().Broker;

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
                    cedearsStockHolding
                        .SetPurchaseUsd(cedearsStockHolding.Cedear.Ratio, cedearsStockHolding.Broker.Comision)
                        .SetCurrentUsd((decimal)cedear.Price);

                    var cedearsStockHoldingDto = mapper.Map<Domain.CedearsStockHolding, CedearStockHoldingDto>(cedearsStockHolding);

                    decimal comision = ((decimal)cedear.Price * broker.Comision) / 100;
                    cedearsStockHoldingDto.CurrentPriceUsd = ((decimal)cedear.Price - comision);
                    cedearWithStockHolding.CedearsStockHoldings.Add(cedearsStockHoldingDto);
                }

                cedearWithStockHoldings.Add(cedearWithStockHolding);
            }

            return cedearWithStockHoldings;
        }
    }
}
