using CalculadoraCedears.Api.Application.Base;
using CalculadoraCedears.Api.Infrastructure.Repositories;

using CalculadoraCedears.Api.Domian;
using CalculadoraCedears.Api.Domian.Events;

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
            var sample = new Cedear(command.request.Description, command.request.Ticker);

            sample.AddDomainEvent(new CedearHasBeenInserted(sample.Id));

            cedearRepository.Add(sample);

            await Commit(cedearRepository.UnitOfWork);
        }
    }
}