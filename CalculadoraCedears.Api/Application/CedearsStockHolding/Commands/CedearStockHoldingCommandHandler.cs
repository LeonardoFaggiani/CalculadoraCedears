using CalculadoraCedears.Api.Application.Base;
using CalculadoraCedears.Api.Infrastructure.Repositories;

using CommunityToolkit.Diagnostics;

using Microsoft.EntityFrameworkCore;

namespace CalculadoraCedears.Api.Application.CedearsStockHolding.Commands
{
    public class CedearStockHoldingCommandHandler : CommandHandler<CedearStockHoldingCommand>
    {
        private readonly ICedearStockHoldingRepository cedearStockHoldingRepository;
        private readonly ICedearRepository cedearRepository;
        private readonly IBrokerRepository brokerRepository;
        private readonly IGoogleFinanceRepository googleFinanceRepository;

        public CedearStockHoldingCommandHandler(ICedearStockHoldingRepository cedearStockHoldingRepository,
            ICedearRepository cedearRepository,
            IBrokerRepository brokerRepository,
            IGoogleFinanceRepository googleFinanceRepository)
        {
            Guard.IsNotNull(cedearStockHoldingRepository, nameof(cedearStockHoldingRepository));
            Guard.IsNotNull(cedearRepository, nameof(cedearRepository));
            Guard.IsNotNull(brokerRepository, nameof(brokerRepository));
            Guard.IsNotNull(googleFinanceRepository, nameof(googleFinanceRepository));

            this.cedearStockHoldingRepository = cedearStockHoldingRepository;
            this.cedearRepository = cedearRepository;
            this.brokerRepository = brokerRepository;
            this.googleFinanceRepository = googleFinanceRepository;
        }

        protected override async Task Handle(CedearStockHoldingCommand command, CancellationToken cancellationToken)
        {
            await this.cedearStockHoldingRepository.TryIfAlreadyExistsAsync(command.request.SinceDate, command.request.CedearId, cancellationToken);

            var cedearsStockHolding = new Domain.CedearsStockHolding(command.request.Quantity, command.request.SinceDate, command.request.ExchangeRateCcl, command.request.PurchasePriceArs);

            var cedear = await this.cedearRepository.All().FirstAsync(x => x.Id == command.request.CedearId, cancellationToken);
            var broker = await this.brokerRepository.All().FirstAsync(x => x.Id == command.request.BrokerId, cancellationToken);

            var googleFinance = await this.googleFinanceRepository.TryGetCurrentPriceByTickerAndMarketAsync(cedear.Ticker, cedear.Market, cancellationToken);

            cedearsStockHolding.SetBroker(broker)
            .SetCedear(cedear)
                .SetEffectiveRatio(cedear.Ratio)
                .SetPurchaseUsd(cedear.Ratio, broker.Comision)
                .SetCurrentUsd(googleFinance.Price);


            this.cedearStockHoldingRepository.Add(cedearsStockHolding);

            await Commit(this.cedearStockHoldingRepository.UnitOfWork);
        }
    }
}