using CalculadoraCedears.Api.Domain;
using CalculadoraCedears.Api.Infrastructure.Data;
using CalculadoraCedears.Api.Infrastructure.Repositories.Base;

namespace CalculadoraCedears.Api.Infrastructure.Repositories
{
    public interface IUserRepository : IRepository<User>
    { }

    public class UserRepository : Repository<User>, IUserRepository
    {

        public UserRepository(CalculadoraCedearsContext context) : base(context)
        { }
    }
}
