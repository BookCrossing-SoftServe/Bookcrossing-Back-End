using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.IRepositories;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Infastructure
{
    public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntityBase
    {
        protected readonly BookCrossingContext _context;

        public BaseRepository(BookCrossingContext context)
        {
            _context = context;
        }

        public virtual async Task AddAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
        }

        public virtual async Task<TEntity> FindByIdAsync(params object[] keys)
        {
            return await _context.Set<TEntity>().FindAsync(keys);
        }

        public virtual async Task<List<TEntity>> GetAllAsync()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return _context.Set<TEntity>().AsQueryable();
        }

        public virtual void Remove(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
        }

        public virtual void Update(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
