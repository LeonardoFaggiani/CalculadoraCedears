using AutoMapper;

using CaluladoraCedears.Api.Domian;
using CaluladoraCedears.Api.Dto;
using CaluladoraCedears.Api.Dto.Samples;
using CaluladoraCedears.Api.Infrastructure.Repositories;

using CommunityToolkit.Diagnostics;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace CaluladoraCedears.Api.Application.Samples.Queries
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
            IEnumerable<Cedear> samples = await this.cedearRepository.All().ToListAsync(cancellationToken);

            var samplesDto = this.mapper.Map<IEnumerable<Cedear>, IEnumerable<CedaerDto>>(samples);

            return new CedearsQueryResponse(samplesDto);
        }
    }
}