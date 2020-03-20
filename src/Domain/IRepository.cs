using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.IRepositories
{
    public interface IRepository<TEntity> where TEntity : IEntityBase
    {
        IQueryable<TEntity> GetAll();
        Task<List<TEntity>> GetAllAsync();
        Task AddAsync(TEntity entity);
        Task<TEntity> FindByIdAsync(params object[] keys);
        void Remove(TEntity entity);
        void Update(TEntity entity);
        Task SaveChangesAsync();
    }
}
