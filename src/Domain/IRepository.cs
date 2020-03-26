using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public interface IRepository<TEntity> where TEntity : IEntityBase
    {
        IQueryable<TEntity> GetAll();
        Task<List<TEntity>> GetAllAsync();
        Task<TEntity> FindByIdAsync(params object[] keys);
        Task<TEntity> FindByCondition(Expression<Func<TEntity, bool>> predicate);
        void Add(TEntity entity);
        void Remove(TEntity entity);
        void Update(TEntity entity);
        Task SaveChangesAsync();
    }
}
