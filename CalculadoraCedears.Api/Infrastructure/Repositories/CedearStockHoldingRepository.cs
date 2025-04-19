using CalculadoraCedears.Api.CrossCutting.Resources;
using CalculadoraCedears.Api.Domain;
using CalculadoraCedears.Api.Infrastructure.Data;
using CalculadoraCedears.Api.Infrastructure.Exceptions;
using CalculadoraCedears.Api.Infrastructure.Repositories.Base;

using Microsoft.EntityFrameworkCore;

namespace CalculadoraCedears.Api.Infrastructure.Repositories
{
    public interface ICedearStockHoldingRepository : IRepository<CedearsStockHolding>
    {
        Task TryIfAlreadyExistsAsync(DateTime sinceDate, Guid cedearId, CancellationToken cancellationToken);
    }

    public class CedearStockHoldingRepository : Repository<CedearsStockHolding>, ICedearStockHoldingRepository
    {
        public CedearStockHoldingRepository(CalculadoraCedearsContext context) : base(context)
        { }

        public async Task TryIfAlreadyExistsAsync(DateTime sinceDate, Guid cedearId, CancellationToken cancellationToken)
        {
            var cedearStockHolding = await this.All().FirstOrDefaultAsync(x => x.SinceDate.Date == sinceDate.Date && x.CedearId == cedearId, cancellationToken);

            if (cedearStockHolding is not null)
                throw new AlreadyExistsCedearException(Messages.CedearAlreadyExists);
        }
    }
}