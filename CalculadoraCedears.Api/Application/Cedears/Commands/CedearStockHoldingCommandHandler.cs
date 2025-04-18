using CalculadoraCedears.Api.Application.Base;
using CalculadoraCedears.Api.CrossCutting.Resources;
using CalculadoraCedears.Api.Domian;
using CalculadoraCedears.Api.Infrastructure.Exceptions;
using CalculadoraCedears.Api.Infrastructure.Repositories;

using CommunityToolkit.Diagnostics;

using Microsoft.EntityFrameworkCore;

namespace CalculadoraCedears.Api.Application.Cedears.Commands
{
    public class CedearStockHoldingCommandHandler : CommandHandler<CedearStockHoldingCommand>
    {
        private readonly ICedearStockHoldingRepository cedearStockHoldingRepository;
        private readonly ICedearRepository cedearRepository;
        private readonly IBrokerRepository brokerRepository;

        public CedearStockHoldingCommandHandler(ICedearStockHoldingRepository cedearStockHoldingRepository,
            ICedearRepository cedearRepository,
            IBrokerRepository brokerRepository)
        {
            Guard.IsNotNull(cedearStockHoldingRepository, nameof(cedearStockHoldingRepository));
            Guard.IsNotNull(cedearRepository, nameof(cedearRepository));
            Guard.IsNotNull(brokerRepository, nameof(brokerRepository));

            this.cedearStockHoldingRepository = cedearStockHoldingRepository;
            this.cedearRepository = cedearRepository;
            this.brokerRepository = brokerRepository;
        }

        protected override async Task Handle(CedearStockHoldingCommand command, CancellationToken cancellationToken)
        {
            var alreadyExsits = await this.cedearStockHoldingRepository.VerifyIfAlreadyExistsAsync(command.request.SinceDate, command.request.CedearId, cancellationToken);

            if (alreadyExsits)
                throw new AlreadyExistsCedearException(Messages.CedearAlreadyExists);

            var cedearsStockHolding = new CedearsStockHolding(command.request.Quantity, command.request.SinceDate, command.request.ExchangeRateCCL, command.request.PurchasePriceArs);

            var cedear = await this.cedearRepository.All().FirstAsync(x => x.Id == command.request.CedearId, cancellationToken);
            var broker = await this.brokerRepository.All().FirstAsync(x => x.Id == command.request.BrokerId, cancellationToken);

            cedearsStockHolding.SetBroker(broker.Id)
                .SetCedear(cedear.Id)
                .SetEffectiveRatio(cedear.Ratio)
                .SetPurchaseUsd(cedear.Ratio, broker.Comision);

            this.cedearStockHoldingRepository.Add(cedearsStockHolding);

            await Commit(cedearStockHoldingRepository.UnitOfWork);
        }
    }
}