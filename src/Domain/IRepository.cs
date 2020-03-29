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
        /// <summary>
        /// Enables further filtering by returning IQueryable
        /// </summary>
        /// <returns>entity framework's entity</returns>
        IQueryable<TEntity> GetAll();

        /// <summary>
        /// Asynchronously retrieves all of the entities upon call
        /// </summary>
        /// <returns>List of the entity framework's entities</returns>
        Task<List<TEntity>> GetAllAsync();

        /// <summary>
        /// Finds and returns TEntity based on Primary Key
        /// </summary>
        /// <param name="keys">Primary Keys</param>
        /// <returns>entity framework's entity</returns>
        Task<TEntity> FindByIdAsync(params object[] keys);
        /// <summary>
        /// Finds and returns TEntity based on the predicate
        /// </summary>
        /// <param name="predicate">customized condition</param>
        /// <returns>entity framework's entity</returns>
        Task<TEntity> FindByCondition(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Adds entity into DBContext
        /// </summary>
        /// <param name="entity">entity framework's entity</param>
        void Add(TEntity entity);
        /// <summary>
        /// Adds multiple entities into DBContext
        /// </summary>
        /// <param name="entity">entity framework's entity</param>
        void AddRange(IEnumerable<TEntity> entity);
        /// <summary>
        /// Removes entities from DBContext
        /// </summary>
        /// <param name="entity">entity framework's entity</param>
        void Remove(TEntity entity);
        /// <summary>
        /// Removes multiple entities from DBContext
        /// </summary>
        /// <param name="entity">entity framework's entity</param>
        void RemoveRange(IEnumerable<TEntity> entity);
        /// <summary>
        /// Updates existing entity or creates a new one
        /// </summary>
        /// <param name="entity">entity framework's entity</param>
        void Update(TEntity entity);
        /// <summary>
        /// Save changes into the database
        /// </summary>
        Task SaveChangesAsync();
    }
}
