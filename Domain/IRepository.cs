using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.IRepositories
{
    public interface IRepository<TEntity, TKey> where TEntity : IEntityBase
    {
        IEnumerable<TEntity> GetAll();
        void Add(TEntity entity);
        TEntity FindById(TKey id);
        void RemoveById(TEntity entity);
        void Update(TEntity entity);
    }
}
