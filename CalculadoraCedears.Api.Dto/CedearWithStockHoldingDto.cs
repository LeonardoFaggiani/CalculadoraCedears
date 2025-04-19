using System.Collections.Generic;

namespace CalculadoraCedears.Api.Dto
{
    public class CedearWithStockHoldingDto : CedearDto
    {
        public CedearWithStockHoldingDto()
        {
            this.CedearsStockHoldings = new List<CedearStockHoldingDto>();
        }

        public List<CedearStockHoldingDto> CedearsStockHoldings { get; set; }
    }
}
