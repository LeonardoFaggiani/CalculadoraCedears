using System.Collections.Generic;

namespace CalculadoraCedears.Api.Dto.Cedears
{
    public class SearchCedearsByTickerQueryResponse
    {
        public SearchCedearsByTickerQueryResponse(IEnumerable<CedaerDto> cedaerDtos)
        {
            Cedears = cedaerDtos;
        }

        public IEnumerable<CedaerDto> Cedears { get; }
    }
}