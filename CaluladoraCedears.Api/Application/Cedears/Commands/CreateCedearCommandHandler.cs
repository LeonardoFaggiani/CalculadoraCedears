using CaluladoraCedears.Api.Application.Base;
using CaluladoraCedears.Api.Domian;
using CaluladoraCedears.Api.Domian.Events;
using CaluladoraCedears.Api.Infrastructure.Repositories;

using CommunityToolkit.Diagnostics;

namespace CaluladoraCedears.Api.Application.Samples.Commands
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

            this.cedearRepository.Add(sample);

            await Commit(this.cedearRepository.UnitOfWork);
        }
    }
}