using System.Collections.Generic;

namespace CalculadoraCedears.Api.Dto.Brokers
{
    public class BrokerQueryResponse
    {
        public BrokerQueryResponse(IEnumerable<BrokerDto> brokers)
        {
            this.Brokers = brokers;
        }

        public IEnumerable<BrokerDto> Brokers { get; }
    }
}
