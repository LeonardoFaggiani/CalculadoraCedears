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
        Task TryIfAlreadyExistsAsync(DateTime sinceDate, Guid cedearId, string userId, CancellationToken cancellationToken);
        Task<Dictionary<string, List<Domain.CedearsStockHolding>>> GetActivesAndGroupedByTickerAsync(string userId, CancellationToken cancellationToken, bool onlyPriceChanged = false);
    }

    public class CedearStockHoldingRepository : Repository<CedearsStockHolding>, ICedearStockHoldingRepository
    {
        public CedearStockHoldingRepository(CalculadoraCedearsContext context) : base(context)
        { }

        public async Task TryIfAlreadyExistsAsync(DateTime sinceDate, Guid cedearId, string userId, CancellationToken cancellationToken)
        {
            var cedearStockHolding = await this.All().Include(x => x.User)
                .FirstOrDefaultAsync(x => x.SinceDate.Date == sinceDate.Date && x.CedearId == cedearId && x.User.ThirdPartyUserId.ToLower() == userId, cancellationToken);

            if (cedearStockHolding is not null)
                throw new AlreadyExistsCedearException(Messages.CedearAlreadyExists);
        }

        public async Task<Dictionary<string, List<Domain.CedearsStockHolding>>> GetActivesAndGroupedByTickerAsync(string userId, CancellationToken cancellationToken, bool onlyPriceChanged = false)
        {
            Expression<Func<CedearsStockHolding, bool>> customPredicate = (x => x.UntilDate == null && x.User.ThirdPartyUserId.ToLower() == userId.ToLower());

            if (onlyPriceChanged)
                customPredicate = (x => x.UntilDate == null && x.Cedear.PriceHasBeenChanged && x.User.ThirdPartyUserId.ToLower() == userId.ToLower());

            var cedearStockHoldings = await this.All()
                .Include(x => x.Cedear)
                .Include(x => x.Broker)
                .Include(x => x.User)
                .Where(customPredicate).ToListAsync(cancellationToken);

            return cedearStockHoldings.GroupBy(c => c.Cedear.Ticker).ToDictionary(group => group.Key, group => group.ToList());
        }
    }
}