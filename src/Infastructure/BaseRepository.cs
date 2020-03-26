using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntityBase
    {
        protected readonly BookCrossingContext Context;
        protected readonly DbSet<TEntity> Entities;

        public BaseRepository(BookCrossingContext context)
        {
            Context = context;
            Entities = context.Set<TEntity>();
        }
        public virtual async Task<List<TEntity>> GetAllAsync()
        {
            return await Entities.ToListAsync();
        }
        public virtual async Task<TEntity> FindByIdAsync(params object[] keys)
        {
            return await Entities.FindAsync(keys);
        }
        public virtual async Task<TEntity> FindByCondition(Expression<Func<TEntity, bool>> predicate)
        {

            return await Entities.FirstOrDefaultAsync(predicate);
        }
        public virtual IQueryable<TEntity> GetAll()
        {
            return Entities.AsQueryable();
        }
        public virtual void Add(TEntity entity)
        {
            Entities.Add(entity);
        }
        public virtual void AddRange(IEnumerable<TEntity> entity)
        {
            Entities.AddRange(entity);
        }
        public virtual void Remove(TEntity entity)
        {
            Entities.Remove(entity);
        }
        public virtual void RemoveRange(IEnumerable<TEntity> entity)
        {
            Entities.RemoveRange(entity);
        }
        public virtual void Update(TEntity entity)
        {
            Entities.Update(entity);
        }

        public async Task SaveChangesAsync()
        {
            await Context.SaveChangesAsync();
        }
    }
}
