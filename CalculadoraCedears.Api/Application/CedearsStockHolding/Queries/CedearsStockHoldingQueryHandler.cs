using AutoMapper;

using CalculadoraCedears.Api.Dto.CedearsStockHolding;
using CalculadoraCedears.Api.Infrastructure.Extensions;
using CalculadoraCedears.Api.Infrastructure.Repositories;

using CommunityToolkit.Diagnostics;

using MediatR;

namespace CalculadoraCedears.Api.Application.CedearsStockHolding.Queries
{
    public class CedearsStockHoldingQueryHandler : IRequestHandler<CedearsStockHoldingQuery, CedearsStockHoldingQueryResponse>
    {
        private readonly ICedearStockHoldingRepository cedearStockHoldingRepository;
        private readonly IMapper mapper;

        public CedearsStockHoldingQueryHandler(ICedearStockHoldingRepository cedearStockHoldingRepository,
            IMapper mapper)
        {
            Guard.IsNotNull(cedearStockHoldingRepository, nameof(cedearStockHoldingRepository));
            Guard.IsNotNull(mapper, nameof(mapper));

            this.cedearStockHoldingRepository = cedearStockHoldingRepository;
            this.mapper = mapper;
        }

        public async Task<CedearsStockHoldingQueryResponse> Handle(CedearsStockHoldingQuery request, CancellationToken cancellationToken)
        {
            var cedearsByTicker = await this.cedearStockHoldingRepository.GetActivesAndGroupedByTickerAsync(cancellationToken);

            return new CedearsStockHoldingQueryResponse(cedearsByTicker.ConvertToResult(this.mapper));
        }
    }
}