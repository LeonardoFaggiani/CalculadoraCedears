using CommunityToolkit.Diagnostics;

using Microsoft.EntityFrameworkCore;

using NetDevPack.Data;
using NetDevPack.Domain;

using CaluladoraCedears.Api.Infrastructure.Data;

namespace CaluladoraCedears.Api.Infrastructure.Repositories.Base
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        protected readonly CaluladoraCedearsContext Context;
        public IUnitOfWork UnitOfWork => Context;

        public Repository(CaluladoraCedearsContext context)
        {
            Guard.IsNotNull(context, nameof(context));

            this.Context = context;
        }

        public IQueryable<TEntity> All()
        {
            DbSet<TEntity> dbEntity = this.Context.Set<TEntity>();

            return dbEntity.AsNoTracking();
        }

        public void Update(TEntity entity)
        {
            this.Context.Update(entity);
        }

        public void Add(TEntity entity)
        {
            this.Context.Add(entity);            
        }
    }
}
