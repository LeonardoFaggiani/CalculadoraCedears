using CalculadoraCedears.Api.Infrastructure.Repositories;

using CommunityToolkit.Diagnostics;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace CalculadoraCedears.Api.Application.CedearsStockHolding.Commands
{
    public class CedearStockHoldingCommandHandler : AsyncRequestHandler<CedearStockHoldingCommand>
    {
        private readonly ICedearStockHoldingRepository cedearStockHoldingRepository;
        private readonly ICedearRepository cedearRepository;
        private readonly IBrokerRepository brokerRepository;
        private readonly IGoogleRepository googleRepository;
        private readonly IUserRepository userRepository;

        public CedearStockHoldingCommandHandler(ICedearStockHoldingRepository cedearStockHoldingRepository,
            ICedearRepository cedearRepository,
            IBrokerRepository brokerRepository,
            IGoogleRepository googleRepository,
            IUserRepository userRepository)
        {
            Guard.IsNotNull(cedearStockHoldingRepository, nameof(cedearStockHoldingRepository));
            Guard.IsNotNull(cedearRepository, nameof(cedearRepository));
            Guard.IsNotNull(brokerRepository, nameof(brokerRepository));
            Guard.IsNotNull(googleRepository, nameof(googleRepository));
            Guard.IsNotNull(userRepository, nameof(userRepository));

            this.cedearStockHoldingRepository = cedearStockHoldingRepository;
            this.cedearRepository = cedearRepository;
            this.brokerRepository = brokerRepository;
            this.googleRepository = googleRepository;
            this.userRepository = userRepository;
        }

        protected override async Task Handle(CedearStockHoldingCommand command, CancellationToken cancellationToken)
        {
            await this.cedearStockHoldingRepository.TryIfAlreadyExistsAsync(command.request.SinceDate, command.request.CedearId, command.request.UserId, cancellationToken);

            var cedearsStockHolding = new Domain.CedearsStockHolding(command.request.Quantity, command.request.SinceDate, command.request.ExchangeRateCcl, command.request.PurchasePriceArs);

            var cedear = await this.cedearRepository.All().FirstAsync(x => x.Id == command.request.CedearId, cancellationToken);
            var broker = await this.brokerRepository.All().FirstAsync(x => x.Id == command.request.BrokerId, cancellationToken);
            var user = await this.userRepository.All().FirstAsync(x => x.ThirdPartyUserId.ToLower() == command.request.UserId.ToLower(), cancellationToken);

            var googleFinance = await this.googleRepository.TryGetFromFinanceCurrentPriceByTickerAndMarketAsync(cedear.Ticker, cedear.Market, cancellationToken);

            cedearsStockHolding.SetBroker(broker)
                .SetCedear(cedear)
                .SetUser(user)
                .SetEffectiveRatio(cedear.Ratio)
                .SetPurchaseUsd(cedear.Ratio, broker.Comision)
                .SetCurrentUsd(googleFinance.Price);

            this.cedearStockHoldingRepository.Add(cedearsStockHolding);

            await this.cedearStockHoldingRepository.UnitOfWork.Commit();
        }
    }
}