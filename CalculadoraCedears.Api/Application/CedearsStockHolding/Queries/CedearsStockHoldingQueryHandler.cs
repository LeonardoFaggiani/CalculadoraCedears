using AutoMapper;

using CalculadoraCedears.Api.Domain;
using CalculadoraCedears.Api.Dto;
using CalculadoraCedears.Api.Dto.CedearsStockHolding;
using CalculadoraCedears.Api.Infrastructure.Repositories;

using CommunityToolkit.Diagnostics;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace CalculadoraCedears.Api.Application.CedearsStockHolding.Queries
{
    public class CedearsStockHoldingQueryHandler : IRequestHandler<CedearsStockHoldingQuery, CedearsStockHoldingQueryResponse>
    {
        private readonly ICedearStockHoldingRepository cedearStockHoldingRepository;
        private readonly IMapper mapper;
        private Dictionary<string, List<Domain.CedearsStockHolding>> cedearsByTicker;

        public CedearsStockHoldingQueryHandler(ICedearStockHoldingRepository cedearStockHoldingRepository,
            IMapper mapper)
        {
            Guard.IsNotNull(cedearStockHoldingRepository, nameof(cedearStockHoldingRepository));
            Guard.IsNotNull(mapper, nameof(mapper));

            this.cedearStockHoldingRepository = cedearStockHoldingRepository;
            this.mapper = mapper;
            this.cedearsByTicker = new Dictionary<string, List<Domain.CedearsStockHolding>>();
        }

        public async Task<CedearsStockHoldingQueryResponse> Handle(CedearsStockHoldingQuery request, CancellationToken cancellationToken)
        {
            var cedearStockHoldings = await this.cedearStockHoldingRepository.All()
                .Include(x => x.Cedear)
                .Include(x => x.Broker)
                .Where(x => x.UntilDate == null).ToListAsync(cancellationToken);

            this.cedearsByTicker = cedearStockHoldings.GroupBy(c => c.Cedear.Ticker).ToDictionary(group => group.Key, group => group.ToList());

            var cedearWithStockHoldings = new List<CedearWithStockHoldingDto>();

            foreach (var ticker in cedearsByTicker.Keys)
            {
                var cedear = cedearsByTicker[ticker].First().Cedear;

                var cedearWithStockHolding = new CedearWithStockHoldingDto
                {
                    Ticker = ticker,
                    Ratio = cedear.Ratio,
                    Id = cedear.Id,
                    Name = cedear.Name
                };

                foreach (var cedearsStockHolding in cedearsByTicker[ticker])
                    cedearWithStockHolding.CedearsStockHoldings.Add(this.mapper.Map<Domain.CedearsStockHolding, CedearStockHoldingDto>(cedearsStockHolding));

                cedearWithStockHoldings.Add(cedearWithStockHolding);
            }

            return new CedearsStockHoldingQueryResponse(cedearWithStockHoldings);
        }
    }
}