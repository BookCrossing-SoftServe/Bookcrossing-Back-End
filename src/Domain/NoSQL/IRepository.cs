using Domain.NoSQL.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Domain.NoSQL
{
    public interface IRepository<TEntity> where TEntity : class, IEntityBase, new()
    {
        /// <summary>
        /// Inserts one entity in collection
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>Number of inserted entities</returns>
        Task<int> InsertOneAsync(TEntity entity);

        /// <summary>
        /// Inserts one entity in collection
        /// </summary>
        /// <param name="entities">Entities</param>
        /// <returns>Number of inserted entities</returns>
        Task<int> InsertManyAsync(params TEntity[] entities);

        /// <summary>
        /// Get all entities from collection
        /// </summary>
        /// <returns>Ienumerable of entities</returns>
        Task<IEnumerable<TEntity>> GetAllAsync();

        /// <summary>
        /// Get entity from collection
        /// </summary>
        /// <param name="id">Document id</param>
        /// <returns>Entity</returns>
        Task<TEntity> FindByIdAsync(ObjectId id);

        /// <summary>
        /// Get entity from collection
        /// </summary>
        /// <param name="filter">Entity</param>
        /// <returns>Entity</returns>
        Task<TEntity> FindOneAsync(TEntity filter);

        /// <summary>
        /// Get entity from collection
        /// </summary>
        /// <param name="predicate">Condition</param>
        /// <returns>Entity</returns>
        Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Get entities from collection
        /// </summary>
        /// <param name="filter">Entity</param>
        /// <returns>Ienumerable of entities</returns>
        Task<IEnumerable<TEntity>> FindManyAsync(TEntity filter);

        /// <summary>
        /// Get entities from collection
        /// </summary>
        /// <param name="predicate">Condition</param>
        /// <returns>Ienumerable of entities</returns>
        Task<IEnumerable<TEntity>> FindManyAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Delete entity from collection
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>DeleteResult</returns>
        Task<DeleteResult> DeleteByIdAsync(ObjectId id);

        /// <summary>
        /// Delete entity from collection
        /// </summary>
        /// <param name="predicate">Condition</param>
        /// <returns>DeleteResult</returns>
        Task<DeleteResult> DeleteOneAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Delete entity from collection
        /// </summary>
        /// <param name="filter">Entity</param>
        /// <returns>DeleteResult</returns>
        Task<DeleteResult> DeleteOneAsync(TEntity filter);

        /// <summary>
        /// Delete entities from collection
        /// </summary>
        /// <param name="predicate">Condition</param>
        /// <returns>DeleteResult</returns>
        Task<DeleteResult> DeleteManyAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Delete entities from collection
        /// </summary>
        /// <param name="filter">Entities</param>
        /// <returns>DeleteResult</returns>
        Task<DeleteResult> DeleteManyAsync(TEntity filter);

        /// <summary>
        /// Update entity in collection
        /// </summary>
        /// <param name="id">Entities</param>
        /// <param name="entity">Entity with new properies</param>
        /// <returns>UpdateResult</returns>
        Task<UpdateResult> UpdateByIdAsync(ObjectId id, TEntity entity);

        /// <summary>
        /// Update entity in collection
        /// </summary>
        /// <param name="predicate">Condition</param>
        /// <param name="entity">Entity with new properies</param>
        /// <returns>UpdateResult</returns>
        Task<UpdateResult> UpdateOneAsync(Expression<Func<TEntity, bool>> predicate, TEntity entity);

        /// <summary>
        /// Update entity in collection
        /// </summary>
        /// <param name="filter">Entity</param>
        /// <param name="entity">Entity with new properies</param>
        /// <returns>UpdateResult</returns>
        Task<UpdateResult> UpdateOneAsync(TEntity filter, TEntity entity);

        /// <summary>
        /// Update entities in collection
        /// </summary>
        /// <param name="predicate">Condition</param>
        /// <param name="entity">Entity with new properies</param>
        /// <returns>UpdateResult</returns>
        Task<UpdateResult> UpdateManyAsync(Expression<Func<TEntity, bool>> predicate, TEntity entity);

        /// <summary>
        /// Update entities in collection
        /// </summary>
        /// <param name="filter">Entity</param>
        /// <param name="entity">Entity with new properies</param>
        /// <returns>UpdateResult</returns>
        Task<UpdateResult> UpdateManyAsync(TEntity filter, TEntity entity);

    }
}
