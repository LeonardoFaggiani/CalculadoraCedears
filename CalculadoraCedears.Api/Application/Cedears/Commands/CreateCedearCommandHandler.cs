using CalculadoraCedears.Api.Application.Base;
using CalculadoraCedears.Api.Domian;
using CalculadoraCedears.Api.Domian.Events;
using CalculadoraCedears.Api.Infrastructure.Repositories;

using CommunityToolkit.Diagnostics;

namespace CalculadoraCedears.Api.Application.Cedears.Commands
{
    public class CreateCedearCommandHandler : CommandHandler<CreateCedearCommand>
    {
        private readonly ICedearRepository cedearRepository;

        public CreateCedearCommandHandler(ICedearRepository cedearRepository)
        {
            Guard.IsNotNull(cedearRepository, nameof(cedearRepository));

            this.cedearRepository = cedearRepository;
        }

        protected override async Task Handle(CreateCedearCommand command, CancellationToken cancellationToken)
        {
            var cedear = new Cedear(command.request.Name, command.request.Ticker,1);

            cedear.AddDomainEvent(new CedearHasBeenInserted(cedear.Id));

            cedearRepository.Add(cedear);

            await Commit(cedearRepository.UnitOfWork);
        }
    }
}