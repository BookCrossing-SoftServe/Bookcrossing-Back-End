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

namespace Infastructure
{
    public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntityBase
    {
        protected readonly BookCrossingContext _context;
        private DbSet<TEntity> entities;

        public BaseRepository(BookCrossingContext context)
        {
            _context = context;
            entities = context.Set<TEntity>();
        }

        public virtual void Add(TEntity entity)
        {
            entities.Add(entity);
        }
        public virtual async Task<List<TEntity>> GetAllAsync()
        {
            return await entities.ToListAsync();
        }

        public virtual async Task<TEntity> FindByIdAsync(params object[] keys)
        {
            return await entities.FindAsync(keys);
        }

        public Task<TEntity> FindByCondition(Expression<Func<TEntity, bool>> predicate)
        {

            return entities.FirstOrDefaultAsync(predicate);
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return entities.AsQueryable();
        }

        public virtual void Remove(TEntity entity)
        {
            entities.Remove(entity);
        }

        public virtual void Update(TEntity entity)
        {
            entities.Update(entity);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
