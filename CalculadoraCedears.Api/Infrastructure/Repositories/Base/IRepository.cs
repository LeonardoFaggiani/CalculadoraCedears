using NetDevPack.Data;
using NetDevPack.Domain;

namespace CalculadoraCedears.Api.Infrastructure.Repositories.Base
{
    public interface IRepository<TEntity> where TEntity : Entity
    {
        IUnitOfWork UnitOfWork { get; }
        void Add(TEntity entity);
        IQueryable<TEntity> All();
        void Update(TEntity entity);
        void Delete(TEntity entity);
    }
}
