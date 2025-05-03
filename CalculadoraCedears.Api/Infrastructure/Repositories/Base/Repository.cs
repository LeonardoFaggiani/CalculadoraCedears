using CalculadoraCedears.Api.Infrastructure.Data;

using CommunityToolkit.Diagnostics;

using Microsoft.EntityFrameworkCore;

using NetDevPack.Data;
using NetDevPack.Domain;

namespace CalculadoraCedears.Api.Infrastructure.Repositories.Base
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        protected readonly CalculadoraCedearsContext Context;
        public IUnitOfWork UnitOfWork => Context;

        public Repository(CalculadoraCedearsContext context)
        {
            Guard.IsNotNull(context, nameof(context));

            Context = context;
        }

        public IQueryable<TEntity> All()
        {
            DbSet<TEntity> dbEntity = Context.Set<TEntity>();

            return dbEntity;
        }

        public void Update(TEntity entity)
        {
            Context.Update(entity);
        }

        public void Add(TEntity entity)
        {
            Context.Add(entity);
        }

        public void Delete(TEntity entity)
        {
            Context.Remove(entity);
        }
    }
}
