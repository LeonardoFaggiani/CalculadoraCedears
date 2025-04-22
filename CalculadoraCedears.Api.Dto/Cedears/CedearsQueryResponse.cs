using System.Collections.Generic;

namespace CalculadoraCedears.Api.Dto.Cedears
{
    public class CedearsQueryResponse
    {
        public CedearsQueryResponse(IEnumerable<CedearDto> cedaerDtos)
        {
            Cedears = cedaerDtos;
        }

        public IEnumerable<CedearDto> Cedears { get; }
    }
}