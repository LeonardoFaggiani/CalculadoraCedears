using CalculadoraCedears.Api.Domain;
using CalculadoraCedears.Api.Infrastructure.Data;
using CalculadoraCedears.Api.Infrastructure.Repositories.Base;

namespace CalculadoraCedears.Api.Infrastructure.Repositories
{
    public interface ICedearRepository : IRepository<Cedear>
    { }

    public class CedearRepository : Repository<Cedear>, ICedearRepository
    {

        public CedearRepository(CalculadoraCedearsContext context) : base(context)
        { }
    }
}
