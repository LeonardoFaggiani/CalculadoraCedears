using System.Collections.Generic;

using CalculadoraCedears.Api.Dto;

namespace CalculadoraCedears.Api.Dto.Samples
{
    public class CedearsQueryResponse
    {
        public CedearsQueryResponse(IEnumerable<CedaerDto> samples)
        {
            Samples = samples;
        }

        public IEnumerable<CedaerDto> Samples { get; }
    }
}