using AutoMapper;

using CalculadoraCedears.Api.Application.Base;
using CalculadoraCedears.Api.Infrastructure.Repositories;

using CommunityToolkit.Diagnostics;

using Microsoft.EntityFrameworkCore;

namespace CalculadoraCedears.Api.Application.CedearsStockHolding.Commands
{
    public class UpdateCedearStockHoldingCommandHandler : CommandHandler<UpdateCedearStockHoldingCommand>
    {
        private readonly ICedearStockHoldingRepository cedearStockHoldingRepository;
        private readonly IMapper mapper;

        public UpdateCedearStockHoldingCommandHandler(ICedearStockHoldingRepository cedearStockHoldingRepository,
            IMapper mapper)
        {
            Guard.IsNotNull(cedearStockHoldingRepository, nameof(cedearStockHoldingRepository));
            Guard.IsNotNull(mapper, nameof(mapper));

            this.cedearStockHoldingRepository = cedearStockHoldingRepository;
            this.mapper = mapper;
        }

        protected override async Task Handle(UpdateCedearStockHoldingCommand command, CancellationToken cancellationToken)
        {
            Domain.CedearsStockHolding cedearStockHolding = await this.cedearStockHoldingRepository.All()
                .Include(x => x.Broker)
                .FirstAsync(x => x.Id.Equals(command.request.Id), cancellationToken);

            this.mapper.Map(command.request, cedearStockHolding);

            this.cedearStockHoldingRepository.Update(cedearStockHolding);

            await Commit(this.cedearStockHoldingRepository.UnitOfWork);
        }
    }
}