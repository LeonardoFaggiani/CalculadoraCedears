using AutoMapper;

using CalculadoraCedears.Api.Domain;
using CalculadoraCedears.Api.Infrastructure.Repositories;

using CommunityToolkit.Diagnostics;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace CalculadoraCedears.Api.Application.CedearsStockHolding.Commands
{
    public class UpdateCedearStockHoldingCommandHandler : AsyncRequestHandler<UpdateCedearStockHoldingCommand>
    {
        private readonly ICedearStockHoldingRepository cedearStockHoldingRepository;
        private readonly IGoogleRepository googleRepository;
        private readonly IMapper mapper;

        public UpdateCedearStockHoldingCommandHandler(ICedearStockHoldingRepository cedearStockHoldingRepository,
            IGoogleRepository googleRepository,
            IMapper mapper)
        {
            Guard.IsNotNull(cedearStockHoldingRepository, nameof(cedearStockHoldingRepository));
            Guard.IsNotNull(googleRepository, nameof(googleRepository));
            Guard.IsNotNull(mapper, nameof(mapper));

            this.cedearStockHoldingRepository = cedearStockHoldingRepository;
            this.googleRepository = googleRepository;
            this.mapper = mapper;
        }

        protected override async Task Handle(UpdateCedearStockHoldingCommand command, CancellationToken cancellationToken)
        {
            Domain.CedearsStockHolding cedearStockHolding = await this.cedearStockHoldingRepository.All()
                .Include(x => x.Broker)
                .Include(x => x.Cedear)
                .FirstAsync(x => x.Id.Equals(command.request.Id), cancellationToken);

            this.mapper.Map(command.request, cedearStockHolding);

            var googleFinance = await this.googleRepository.TryGetFromFinanceCurrentPriceByTickerAndMarketAsync(cedearStockHolding.Cedear.Ticker, cedearStockHolding.Cedear.Market, cancellationToken);

            cedearStockHolding.SetPurchaseUsd(cedearStockHolding.Cedear.Ratio, cedearStockHolding.Broker.Comision)
                .SetCurrentUsd(googleFinance.Price);

            this.cedearStockHoldingRepository.Update(cedearStockHolding);

            await this.cedearStockHoldingRepository.UnitOfWork.Commit();
        }
    }
}