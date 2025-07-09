using CalculadoraCedears.Api.Domain;
using CalculadoraCedears.Api.Dto.Cedears;
using CalculadoraCedears.Api.Infrastructure.Repositories;

using CommunityToolkit.Diagnostics;

using MediatR;

namespace CalculadoraCedears.Api.Application.Cedears.Queries
{
    public class DollarCCLQueryHandler : IRequestHandler<DollarCCLQuery, DollarCCLQueryResponse>
    {
        private readonly IGoogleRepository googleRepository;

        public DollarCCLQueryHandler(IGoogleRepository googleRepository)
        {
            Guard.IsNotNull(googleRepository, nameof(googleRepository));

            this.googleRepository = googleRepository;
        }

        public async Task<DollarCCLQueryResponse> Handle(DollarCCLQuery query, CancellationToken cancellationToken)
        {
            DollarCCLQuote dollarCCLQuote = await googleRepository.TryGetCurrentDollarCCLQuoteAsync(cancellationToken);

            return new DollarCCLQueryResponse((decimal)dollarCCLQuote.PriceCCL!, dollarCCLQuote.VariationCCL);
        }
    }
}