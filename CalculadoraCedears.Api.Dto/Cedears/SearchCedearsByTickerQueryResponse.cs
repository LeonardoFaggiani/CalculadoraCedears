using System.Collections.Generic;

namespace CalculadoraCedears.Api.Dto.Cedears
{
    public class SearchCedearsByTickerQueryResponse
    {
        public SearchCedearsByTickerQueryResponse(IEnumerable<CedearDto> cedaerDtos)
        {
            Cedears = cedaerDtos;
        }

        public IEnumerable<CedearDto> Cedears { get; }
    }
}