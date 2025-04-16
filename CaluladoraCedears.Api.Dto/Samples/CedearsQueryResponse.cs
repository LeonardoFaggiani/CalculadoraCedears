using System.Collections.Generic;

namespace CaluladoraCedears.Api.Dto.Samples
{
    public class CedearsQueryResponse
    {
        public CedearsQueryResponse(IEnumerable<CedaerDto> samples)
        {
            this.Samples = samples;
        }

        public IEnumerable<CedaerDto> Samples { get; }
    }
}