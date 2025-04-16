using AutoMapper;

using CalculadoraCedears.Api.Domian;
using CalculadoraCedears.Api.Dto;
using CalculadoraCedears.Api.Dto.Samples;
using CalculadoraCedears.Api.Infrastructure.Repositories;

using CommunityToolkit.Diagnostics;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace CalculadoraCedears.Api.Application.Cedears.Queries
{
    public class CedearsQueryHandler : IRequestHandler<CedearsQuery, CedearsQueryResponse>
    {
        private readonly ICedearRepository cedearRepository;
        private readonly IMapper mapper;

        public CedearsQueryHandler(ICedearRepository cedearRepository,
            IMapper mapper)
        {
            Guard.IsNotNull(cedearRepository, nameof(cedearRepository));
            Guard.IsNotNull(mapper, nameof(mapper));

            this.cedearRepository = cedearRepository;
            this.mapper = mapper;
        }

        public async Task<CedearsQueryResponse> Handle(CedearsQuery query, CancellationToken cancellationToken)
        {
            IEnumerable<Cedear> samples = await cedearRepository.All().ToListAsync(cancellationToken);

            var samplesDto = mapper.Map<IEnumerable<Cedear>, IEnumerable<CedaerDto>>(samples);

            return new CedearsQueryResponse(samplesDto);
        }
    }
}