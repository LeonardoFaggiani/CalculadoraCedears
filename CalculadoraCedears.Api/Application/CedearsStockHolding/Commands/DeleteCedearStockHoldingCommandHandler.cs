using CalculadoraCedears.Api.Application.Base;
using CalculadoraCedears.Api.Infrastructure.Repositories;

using CommunityToolkit.Diagnostics;

using Microsoft.EntityFrameworkCore;

namespace CalculadoraCedears.Api.Application.CedearsStockHolding.Commands
{
    public class DeleteCedearStockHoldingCommandHandler : CommandHandler<DeleteCedearStockHoldingCommand>
    {
        private readonly ICedearStockHoldingRepository cedearStockHoldingRepository;

        public DeleteCedearStockHoldingCommandHandler(ICedearStockHoldingRepository cedearStockHoldingRepository)
        {
            Guard.IsNotNull(cedearStockHoldingRepository, nameof(cedearStockHoldingRepository));

            this.cedearStockHoldingRepository = cedearStockHoldingRepository;
        }

        protected override async Task Handle(DeleteCedearStockHoldingCommand request, CancellationToken cancellationToken)
        {
            Domain.CedearsStockHolding cedearsStockHolding = await this.cedearStockHoldingRepository.All().FirstAsync(x => x.Id.Equals(request.CedearStockHoldingId), cancellationToken);

            if (cedearsStockHolding is not null)
            {
                this.cedearStockHoldingRepository.Delete(cedearsStockHolding);
                await Commit(this.cedearStockHoldingRepository.UnitOfWork);
            }
        }
    }
}
