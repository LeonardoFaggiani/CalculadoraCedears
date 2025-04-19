using CalculadoraCedears.Api.Domain;
using CalculadoraCedears.Api.Infrastructure.Data;
using CalculadoraCedears.Api.Infrastructure.Repositories.Base;

namespace CalculadoraCedears.Api.Infrastructure.Repositories
{
    public interface IBrokerRepository : IRepository<Broker>
    { }

    public class BrokerRepository : Repository<Broker>, IBrokerRepository
    {
        public BrokerRepository(CalculadoraCedearsContext context) : base(context)
        { }
    }
}
