using System.Collections.Generic;

namespace CalculadoraCedears.Api.Dto.CedearsStockHolding
{
    public class CedearsStockHoldingQueryResponse
    {
        public IEnumerable<CedearWithStockHoldingDto> CedearWithStockHoldings { get; }

        public CedearsStockHoldingQueryResponse(IEnumerable<CedearWithStockHoldingDto> cedearWithStockHoldings)
        {
            this.CedearWithStockHoldings = cedearWithStockHoldings;
        }
    }
}
