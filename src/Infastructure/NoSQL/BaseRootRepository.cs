using Domain.NoSQL;
using Domain.NoSQL.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Infrastructure.NoSQL
{
    public class BaseRootRepository<TRootEntity> : IRootRepository<TRootEntity> where TRootEntity : class, IRootEntityBase, new()
    {
        protected IMongoCollection<TRootEntity> _collection = null;

        public BaseRootRepository(IMongoSettings settings)
        {
            _collection = new MongoContext(settings).Collection<TRootEntity>();
        }

        public async Task<int> InsertOneAsync(TRootEntity entity)
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

        public async Task<int> InsertManyAsync(params TRootEntity[] entities)
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

        public async Task<DeleteResult> DeleteByIdAsync(string id)
        {
            return await _collection.DeleteOneAsync(root=> root.Id==id);
        }

        public async Task<DeleteResult> DeleteManyAsync(Expression<Func<TRootEntity, bool>> predicate)
        {
            return await _collection.DeleteManyAsync(predicate);
        }

        public async Task<DeleteResult> DeleteManyAsync(TRootEntity filter)
        {
            return await _collection.DeleteManyAsync(filter.ToBsonDocument());
        }

        public async Task<DeleteResult> DeleteOneAsync(Expression<Func<TRootEntity, bool>> predicate)
        {
            return await _collection.DeleteOneAsync(predicate);
        }

        public async Task<DeleteResult> DeleteOneAsync(TRootEntity filter)
        {
            return await _collection.DeleteOneAsync(filter.ToBsonDocument());
        }

        public async Task<TRootEntity> FindByIdAsync(string id)
        {
            return await _collection.Find(root => root.Id == id).SingleAsync();
        }

        public async Task<IEnumerable<TRootEntity>> FindManyAsync(TRootEntity filter)
        {
            return await _collection.Find(filter.ToBsonDocument()).ToListAsync();
        }

        public async Task<IEnumerable<TRootEntity>> FindManyAsync(Expression<Func<TRootEntity, bool>> predicate)
        {
            return await _collection.Find(predicate).ToListAsync();
        }

        public async Task<TRootEntity> FindOneAsync(TRootEntity filter)
        {
            return await _collection.Find(filter.ToBsonDocument()).SingleAsync();
        }

        public async Task<TRootEntity> FindOneAsync(Expression<Func<TRootEntity, bool>> predicate)
        {
            return await _collection.Find(predicate).SingleAsync();
        }

        public async Task<IEnumerable<TRootEntity>> GetAllAsync()
        {
            return await _collection.Find(Builders<TRootEntity>.Filter.Empty).ToListAsync();
        }

        public async Task<double> GetAvgRatingAsync(int bookId)
        {
            var result = await _collection.Aggregate().Match(new BsonDocument { { "BookId", bookId } }).Match(new BsonDocument("Rating", new BsonDocument("$ne", 0)))
                .Group(new BsonDocument { { "_id", "$BookId" }, { "avg", new BsonDocument("$avg", "$Rating") } }).FirstOrDefaultAsync();
            return result == null ? 0 : Convert.ToDouble(result.GetValue("avg"));
        }

        public async Task<UpdateResult> UpdateByIdAsync(string id, TRootEntity entity)
        {
            return await _collection.UpdateOneAsync(
                root => root.Id == id, 
                new BsonDocument("$set", entity.ToBsonDocument()));
        }

        public async Task<UpdateResult> UpdateManyAsync(Expression<Func<TRootEntity, bool>> predicate, TRootEntity entity)
        {
            return await _collection.UpdateManyAsync(
                predicate, 
                new BsonDocument("$set", entity.ToBsonDocument()));
        }

        public async Task<UpdateResult> UpdateManyAsync(TRootEntity filter, TRootEntity entity)
        {
            return await _collection.UpdateManyAsync(
                filter.ToBsonDocument(), 
                new BsonDocument("$set", entity.ToBsonDocument()));
        }

        public async Task<UpdateResult> UpdateOneAsync(Expression<Func<TRootEntity, bool>> predicate, TRootEntity entity)
        {
            return await _collection.UpdateOneAsync(
                predicate, 
                new BsonDocument("$set", entity.ToBsonDocument()));
        }

        public async Task<UpdateResult> UpdateOneAsync(TRootEntity filter, TRootEntity entity)
        {
            return await _collection.UpdateOneAsync(
                filter.ToBsonDocument(), 
                new BsonDocument("$set", entity.ToBsonDocument()));
        }      
    }
}
