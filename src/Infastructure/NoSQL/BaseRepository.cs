using Domain.NoSQL;
using Domain.NoSQL.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Infrastructure.NoSQL
{
    public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntityBase, new()
    {
        protected IMongoCollection<TEntity> _collection = null;
        public BaseRepository(IMongoSettings settings)
        {
            _collection = new MongoContext(settings).Collection<TEntity>();
        }

        public async Task<int> InsertOneAsync(TEntity entity)
        {
            try
            {
                await _collection.InsertOneAsync(entity);
                return 1;
            }
            catch
            {
                return 0;
            }
        }

        public async Task<int> InsertManyAsync(params TEntity[] entities)
        {
            try
            {
                await _collection.InsertManyAsync(entities);
                return entities.Length;
            }
            catch
            {
                return 0;
            }
        }

        public async Task<DeleteResult> DeleteByIdAsync(ObjectId id)
        {
            return await _collection.DeleteOneAsync(new TEntity() { Id = id }.ToJson());
        }

        public async Task<DeleteResult> DeleteManyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _collection.DeleteManyAsync(predicate);
        }

        public async Task<DeleteResult> DeleteManyAsync(TEntity filter)
        {
            return await _collection.DeleteManyAsync(filter.ToJson());
        }

        public async Task<DeleteResult> DeleteOneAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _collection.DeleteOneAsync(predicate);
        }

        public async Task<DeleteResult> DeleteOneAsync(TEntity filter)
        {
            return await _collection.DeleteOneAsync(filter.ToJson());
        }

        public async Task<TEntity> FindByIdAsync(ObjectId id)
        {
            return await _collection.Find(new TEntity() { Id = id }.ToJson()).SingleAsync();
        }

        public async Task<IEnumerable<TEntity>> FindManyAsync(TEntity filter)
        {
            return await _collection.Find(filter.ToJson()).ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> FindManyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _collection.Find(predicate).ToListAsync();
        }

        public async Task<TEntity> FindOneAsync(TEntity filter)
        {
            return await _collection.Find(filter.ToJson()).SingleAsync();
        }

        public async Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _collection.Find(predicate).SingleAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _collection.Find(Builders<TEntity>.Filter.Empty).ToListAsync();
        }

        public async Task<UpdateResult> UpdateByIdAsync(ObjectId id, TEntity entity)
        {
            return await _collection.UpdateOneAsync(new TEntity() { Id = id }.ToJson(), entity.ToJson());
        }

        public async Task<UpdateResult> UpdateManyAsync(Expression<Func<TEntity, bool>> predicate, TEntity entity)
        {
            return await _collection.UpdateManyAsync(predicate, entity.ToJson());
        }

        public async Task<UpdateResult> UpdateManyAsync(TEntity filter, TEntity entity)
        {
            return await _collection.UpdateManyAsync(filter.ToJson(), entity.ToJson());
        }

        public async Task<UpdateResult> UpdateOneAsync(Expression<Func<TEntity, bool>> predicate, TEntity entity)
        {
            return await _collection.UpdateOneAsync(predicate, entity.ToJson());
        }

        public async Task<UpdateResult> UpdateOneAsync(TEntity filter, TEntity entity)
        {
            return await _collection.UpdateOneAsync(filter.ToJson(), entity.ToJson());
        }
    }
}
