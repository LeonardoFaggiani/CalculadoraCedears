using AutoMapper;

using CalculadoraCedears.Api.Dto.Cedears;
using CalculadoraCedears.Api.Infrastructure.Repositories;

using CommunityToolkit.Diagnostics;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace CalculadoraCedears.Api.Application.Cedears.Queries
{
    public class CedearsQueryHandler : IRequestHandler<CedearsQuery, CedearsQueryResponse>
    {
        private readonly ICedearStockHoldingRepository cedearStockHoldingRepository;
        private readonly IMapper mapper;

        public CedearsQueryHandler(ICedearStockHoldingRepository cedearStockHoldingRepository,
            IMapper mapper)
        {
            Guard.IsNotNull(cedearStockHoldingRepository, nameof(cedearStockHoldingRepository));
            Guard.IsNotNull(mapper, nameof(mapper));

            this.cedearStockHoldingRepository = cedearStockHoldingRepository;
            this.mapper = mapper;
        }

        public async Task<CedearsQueryResponse> Handle(CedearsQuery request, CancellationToken cancellationToken)
        {
            var cedearStockHoldings = await this.cedearStockHoldingRepository.All()
                .Include(x => x.Cedear)
                .Where(x => x.UntilDate == null).ToListAsync(cancellationToken);

            return new CedearsQueryResponse();
        }
    }
}