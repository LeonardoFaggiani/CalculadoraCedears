using CalculadoraCedears.Api.CrossCutting.Resources;
using CalculadoraCedears.Api.Domain;
using CalculadoraCedears.Api.Infrastructure.Data;
using CalculadoraCedears.Api.Infrastructure.Exceptions;
using CalculadoraCedears.Api.Infrastructure.Repositories.Base;

using Microsoft.EntityFrameworkCore;

using System.Linq.Expressions;

namespace CalculadoraCedears.Api.Infrastructure.Repositories
{
    public interface ICedearStockHoldingRepository : IRepository<CedearsStockHolding>
    {
        Task TryIfAlreadyExistsAsync(DateTime sinceDate, Guid cedearId, CancellationToken cancellationToken);
        Task<Dictionary<string, List<Domain.CedearsStockHolding>>> GetActivesAndGroupedByTickerAsync(CancellationToken cancellationToken, bool onlyPriceChanged = false);
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

        public async Task<Dictionary<string, List<Domain.CedearsStockHolding>>> GetActivesAndGroupedByTickerAsync(CancellationToken cancellationToken, bool onlyPriceChanged = false)
        {
            Expression<Func<CedearsStockHolding, bool>> customPredicate = (x => x.UntilDate == null);

            if (onlyPriceChanged)
                customPredicate = (x => x.UntilDate == null && x.Cedear.PriceHasBeenChanged);

            var cedearStockHoldings = await this.All()
                .Include(x => x.Cedear)
                .Include(x => x.Broker)
                .Where(customPredicate).ToListAsync(cancellationToken);

            return cedearStockHoldings.GroupBy(c => c.Cedear.Ticker).ToDictionary(group => group.Key, group => group.ToList());
        }
    }
}