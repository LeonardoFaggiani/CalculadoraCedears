using CaluladoraCedears.Api.Domian;
using CaluladoraCedears.Api.Infrastructure.Data;
using CaluladoraCedears.Api.Infrastructure.Repositories.Base;

namespace CaluladoraCedears.Api.Infrastructure.Repositories
{
    public interface ICedearRepository : IRepository<Cedear>
    { }

    public class CedearRepository : Repository<Cedear>, ICedearRepository
    {   

        public CedearRepository(CaluladoraCedearsContext context) : base(context)
        { }
    }
}
