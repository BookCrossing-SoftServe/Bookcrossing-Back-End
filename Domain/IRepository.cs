using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.IRepositories
{
    public interface IRepository<TEntity, TKey> where TEntity : IEntityBase
    {
        Task<List<TEntity>> GetAll();
        Task AddAsync(TEntity entity);
        Task<TEntity> FindByIdAsync(TKey id);
        void Remove(TEntity entity);
        void Update(TEntity entity);
    }
}
