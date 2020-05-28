using Domain.NoSQL.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Domain.NoSQL
{
    /// <summary>
    /// Generic repository to work with mongodb collections
    /// </summary>
    /// <typeparam name="TRootEntity">Entity</typeparam>
    public interface IRootRepository<TRootEntity> where TRootEntity : class, IRootEntityBase, new()
    {
        /// <summary>
        /// Inserts one entity in collection
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>Number of inserted entities</returns>
        Task<int> InsertOneAsync(TRootEntity entity);

        /// <summary>
        /// Inserts one entity in collection
        /// </summary>
        /// <param name="entities">Entities</param>
        /// <returns>Number of inserted entities</returns>
        Task<int> InsertManyAsync(params TRootEntity[] entities);

        /// <summary>
        /// Get all entities from collection
        /// </summary>
        /// <returns>Ienumerable of entities</returns>
        Task<IEnumerable<TRootEntity>> GetAllAsync();

        /// <summary>
        /// Get average book rating
        /// </summary>
        /// <returns>double from 1 to 5</returns>
        Task<double> GetAvgRatingAsync();

        /// <summary>
        /// Get entity from collection
        /// </summary>
        /// <param name="id">Document id</param>
        /// <returns>Entity</returns>
        Task<TRootEntity> FindByIdAsync(string id);

        /// <summary>
        /// Get entity from collection
        /// </summary>
        /// <param name="filter">Entity</param>
        /// <returns>Entity</returns>
        Task<TRootEntity> FindOneAsync(TRootEntity filter);

        /// <summary>
        /// Get entity from collection
        /// </summary>
        /// <param name="predicate">Condition</param>
        /// <returns>Entity</returns>
        Task<TRootEntity> FindOneAsync(Expression<Func<TRootEntity, bool>> predicate);

        /// <summary>
        /// Get entities from collection
        /// </summary>
        /// <param name="filter">Entity</param>
        /// <returns>Ienumerable of entities</returns>
        Task<IEnumerable<TRootEntity>> FindManyAsync(TRootEntity filter);

        /// <summary>
        /// Get entities from collection
        /// </summary>
        /// <param name="predicate">Condition</param>
        /// <returns>Ienumerable of entities</returns>
        Task<IEnumerable<TRootEntity>> FindManyAsync(Expression<Func<TRootEntity, bool>> predicate);

        /// <summary>
        /// Delete entity from collection
        /// </summary>
        /// <param name="id">Document id</param>
        /// <returns>DeleteResult</returns>
        Task<DeleteResult> DeleteByIdAsync(string id);

        /// <summary>
        /// Delete entity from collection
        /// </summary>
        /// <param name="predicate">Condition</param>
        /// <returns>DeleteResult</returns>
        Task<DeleteResult> DeleteOneAsync(Expression<Func<TRootEntity, bool>> predicate);

        /// <summary>
        /// Delete entity from collection
        /// </summary>
        /// <param name="filter">Entity</param>
        /// <returns>DeleteResult</returns>
        Task<DeleteResult> DeleteOneAsync(TRootEntity filter);

        /// <summary>
        /// Delete entities from collection
        /// </summary>
        /// <param name="predicate">Condition</param>
        /// <returns>DeleteResult</returns>
        Task<DeleteResult> DeleteManyAsync(Expression<Func<TRootEntity, bool>> predicate);

        /// <summary>
        /// Delete entities from collection
        /// </summary>
        /// <param name="filter">Entities</param>
        /// <returns>DeleteResult</returns>
        Task<DeleteResult> DeleteManyAsync(TRootEntity filter);

        /// <summary>
        /// Update entity in collection
        /// </summary>
        /// <param name="id">document id</param>
        /// <param name="entity">Entity with new properies</param>
        /// <returns>UpdateResult</returns>
        Task<UpdateResult> UpdateByIdAsync(string id, TRootEntity entity);

        /// <summary>
        /// Update entity in collection
        /// </summary>
        /// <param name="predicate">Condition</param>
        /// <param name="entity">Entity with new properies</param>
        /// <returns>UpdateResult</returns>
        Task<UpdateResult> UpdateOneAsync(Expression<Func<TRootEntity, bool>> predicate, TRootEntity entity);

        /// <summary>
        /// Update entity in collection
        /// </summary>
        /// <param name="filter">Entity</param>
        /// <param name="entity">Entity with new properies</param>
        /// <returns>UpdateResult</returns>
        Task<UpdateResult> UpdateOneAsync(TRootEntity filter, TRootEntity entity);

        /// <summary>
        /// Update entities in collection
        /// </summary>
        /// <param name="predicate">Condition</param>
        /// <param name="entity">Entity with new properies</param>
        /// <returns>UpdateResult</returns>
        Task<UpdateResult> UpdateManyAsync(Expression<Func<TRootEntity, bool>> predicate, TRootEntity entity);

        /// <summary>
        /// Update entities in collection
        /// </summary>
        /// <param name="filter">Entity</param>
        /// <param name="entity">Entity with new properies</param>
        /// <returns>UpdateResult</returns>
        Task<UpdateResult> UpdateManyAsync(TRootEntity filter, TRootEntity entity);
    }
}
