using AutoMapper;

using CalculadoraCedears.Api.Domain;
using CalculadoraCedears.Api.Dto;
using CalculadoraCedears.Api.Dto.Brokers;
using CalculadoraCedears.Api.Infrastructure.Repositories;

using CommunityToolkit.Diagnostics;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace CalculadoraCedears.Api.Application.Brokers.Queries
{
    public class BrokerQueryHandler : IRequestHandler<BrokerQuery, BrokerQueryResponse>
    {
        private readonly IBrokerRepository brokerRepository;
        private readonly IMapper mapper;

        public BrokerQueryHandler(IBrokerRepository brokerRepository,
            IMapper mapper)
        {
            Guard.IsNotNull(brokerRepository, nameof(brokerRepository));
            Guard.IsNotNull(mapper, nameof(mapper));

            this.brokerRepository = brokerRepository;
            this.mapper = mapper;
        }

        public async Task<BrokerQueryResponse> Handle(BrokerQuery request, CancellationToken cancellationToken)
        {
            var brokers = await this.brokerRepository.All().ToListAsync(cancellationToken);

            var brokersDto = this.mapper.Map<IEnumerable<Broker>, IEnumerable<BrokerDto>>(brokers);

            return new BrokerQueryResponse(brokersDto);
        }
    }
}