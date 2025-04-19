using AutoMapper;

using CalculadoraCedears.Api.Domain;
using CalculadoraCedears.Api.Dto;
using CalculadoraCedears.Api.Dto.Cedears;
using CalculadoraCedears.Api.Infrastructure.Repositories;

using CommunityToolkit.Diagnostics;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace CalculadoraCedears.Api.Application.Cedears.Queries
{
    public class SearchCedearsByTickerQueryHandler : IRequestHandler<SearchCedearsByTickerQuery, SearchCedearsByTickerQueryResponse>
    {
        private readonly ICedearRepository cedearRepository;
        private readonly IMapper mapper;

        public SearchCedearsByTickerQueryHandler(ICedearRepository cedearRepository,
            IMapper mapper)
        {
            Guard.IsNotNull(cedearRepository, nameof(cedearRepository));
            Guard.IsNotNull(mapper, nameof(mapper));

            this.cedearRepository = cedearRepository;
            this.mapper = mapper;
        }

        public async Task<SearchCedearsByTickerQueryResponse> Handle(SearchCedearsByTickerQuery query, CancellationToken cancellationToken)
        {
            IEnumerable<Cedear> cedears = await cedearRepository.All().Where(c => c.Ticker.ToLower().Contains(query.Ticker.ToLower()))
                .ToListAsync();

            var cedaerDtos = mapper.Map<IEnumerable<Cedear>, IEnumerable<CedearDto>>(cedears);

            return new SearchCedearsByTickerQueryResponse(cedaerDtos);
        }
    }
}