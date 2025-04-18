using CalculadoraCedears.Api.Domian;
using CalculadoraCedears.Api.Infrastructure.Data;
using CalculadoraCedears.Api.Infrastructure.Repositories.Base;

using Microsoft.EntityFrameworkCore;

namespace CalculadoraCedears.Api.Infrastructure.Repositories
{
    public interface ICedearStockHoldingRepository : IRepository<CedearsStockHolding>
    {
        Task<bool> VerifyIfAlreadyExistsAsync(DateTime sinceDate, Guid cedearId, CancellationToken cancellationToken);
    }

    public class CedearStockHoldingRepository : Repository<CedearsStockHolding>, ICedearStockHoldingRepository
    {
        public CedearStockHoldingRepository(CalculadoraCedearsContext context) : base(context)
        { }

        public async Task<bool> VerifyIfAlreadyExistsAsync(DateTime sinceDate, Guid cedearId, CancellationToken cancellationToken)
        {
            var cedearStockHolding = await this.All().FirstOrDefaultAsync(x => x.SinceDate.Date == sinceDate.Date && x.CedearId == cedearId, cancellationToken);

            if (cedearStockHolding is not null)
                return true;

            return false;
        }
    }
}
