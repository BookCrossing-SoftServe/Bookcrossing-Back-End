using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.NoSQL;
using Domain.NoSQL.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.NoSQL
{
    public class BaseChildRepository<TRootEntity, TChildEntity> : IChildRepository<TRootEntity, TChildEntity>
        where TRootEntity : class, IRootEntityBase, new()
        where TChildEntity : class, IChildEntityBase, new()
    {
        protected IMongoCollection<TRootEntity> _collection = null;

        public BaseChildRepository(IMongoSettings settings)
        {
            _collection = new MongoContext(settings).Collection<TRootEntity>();
        }

        public async Task<UpdateResult> PullAsync(Expression<Func<TRootEntity, bool>> predicate, TChildEntity childEntity, IEnumerable<(string nestedArrayName, string itemId)> path, string arrName)
        {
            UpdateOptions updateOptions = new UpdateOptions();
            (string pathStr, IEnumerable<ArrayFilterDefinition> arrayFilterDefinitions) = GetPathAndArrayFilterDefinitions(path);

            pathStr += arrName;
            updateOptions.ArrayFilters = arrayFilterDefinitions;

            List<FilterDefinition<TChildEntity>> filterDefinitions = new List<FilterDefinition<TChildEntity>>();

            foreach (var el in childEntity.ToBsonDocument().Elements)
            {
                filterDefinitions.Add(Builders<TChildEntity>.Filter.Eq(el.Name, el.Value));
            }

            var update = Builders<TRootEntity>.Update.PullFilter(pathStr,
                Builders<TChildEntity>.Filter.And(filterDefinitions));

            return await _collection.UpdateManyAsync(predicate, update, updateOptions);
        }

        public async Task<UpdateResult> PullAsync(string rootId, TChildEntity childEntity, IEnumerable<(string nestedArrayName, string itemId)> path, string arrName)
        {
            UpdateOptions updateOptions = new UpdateOptions();
            (string pathStr, IEnumerable<ArrayFilterDefinition> arrayFilterDefinitions) = GetPathAndArrayFilterDefinitions(path);

            pathStr += arrName;
            updateOptions.ArrayFilters = arrayFilterDefinitions;

            List<FilterDefinition<TChildEntity>> filterDefinitions = new List<FilterDefinition<TChildEntity>>();

            foreach (var el in childEntity.ToBsonDocument().Elements)
            {
                filterDefinitions.Add(Builders<TChildEntity>.Filter.Eq(el.Name, el.Value));
            }

            var update = Builders<TRootEntity>.Update.PullFilter(pathStr,
                Builders<TChildEntity>.Filter.And(filterDefinitions));

            return await _collection.UpdateOneAsync(root=>root.Id==rootId, update, updateOptions);
        }

        public async Task<UpdateResult> PullAsync(TRootEntity filterEntity, TChildEntity childEntity, IEnumerable<(string nestedArrayName, string itemId)> path, string arrName)
        {
            UpdateOptions updateOptions = new UpdateOptions();
            (string pathStr, IEnumerable<ArrayFilterDefinition> arrayFilterDefinitions) = GetPathAndArrayFilterDefinitions(path);

            pathStr += arrName;
            updateOptions.ArrayFilters = arrayFilterDefinitions;

            List<FilterDefinition<TChildEntity>> filterDefinitions = new List<FilterDefinition<TChildEntity>>();

            foreach (var el in childEntity.ToBsonDocument().Elements)
            {
                filterDefinitions.Add(Builders<TChildEntity>.Filter.Eq(el.Name, el.Value));
            }

            var update = Builders<TRootEntity>.Update.PullFilter(pathStr,
                Builders<TChildEntity>.Filter.And(filterDefinitions));

            return await _collection.UpdateManyAsync(filterEntity.ToBsonDocument(), update, updateOptions);
        }

        public async Task<UpdateResult> PullAsync(Expression<Func<TRootEntity, bool>> predicate, string childId, IEnumerable<(string nestedArrayName, string itemId)> path, string arrName)
        {
            UpdateOptions updateOptions = new UpdateOptions();
            (string pathStr, IEnumerable<ArrayFilterDefinition> arrayFilterDefinitions) = GetPathAndArrayFilterDefinitions(path);

            pathStr += arrName;
            updateOptions.ArrayFilters = arrayFilterDefinitions;

            List<FilterDefinition<TChildEntity>> filterDefinitions = new List<FilterDefinition<TChildEntity>>();


            filterDefinitions.Add(Builders<TChildEntity>.Filter.Eq(child=>child.Id, childId));
  
            var update = Builders<TRootEntity>.Update.PullFilter(pathStr,
                Builders<TChildEntity>.Filter.And(filterDefinitions));

            return await _collection.UpdateManyAsync(predicate, update, updateOptions);
        }

        public async Task<UpdateResult> PullAsync(string rootId, string childId, IEnumerable<(string nestedArrayName, string itemId)> path, string arrName)
        {
            UpdateOptions updateOptions = new UpdateOptions();
            (string pathStr, IEnumerable<ArrayFilterDefinition> arrayFilterDefinitions) = GetPathAndArrayFilterDefinitions(path);

            pathStr += arrName;
            updateOptions.ArrayFilters = arrayFilterDefinitions;

            List<FilterDefinition<TChildEntity>> filterDefinitions = new List<FilterDefinition<TChildEntity>>();


            filterDefinitions.Add(Builders<TChildEntity>.Filter.Eq(child => child.Id, childId));

            var update = Builders<TRootEntity>.Update.PullFilter(pathStr,
                Builders<TChildEntity>.Filter.And(filterDefinitions));

            return await _collection.UpdateOneAsync(root=>root.Id==rootId, update, updateOptions);
        }

        public async Task<UpdateResult> PullAsync(TRootEntity filterEntity, string childId, IEnumerable<(string nestedArrayName, string itemId)> path, string arrName)
        {
            UpdateOptions updateOptions = new UpdateOptions();
            (string pathStr, IEnumerable<ArrayFilterDefinition> arrayFilterDefinitions) = GetPathAndArrayFilterDefinitions(path);

            pathStr += arrName;
            updateOptions.ArrayFilters = arrayFilterDefinitions;

            List<FilterDefinition<TChildEntity>> filterDefinitions = new List<FilterDefinition<TChildEntity>>();


            filterDefinitions.Add(Builders<TChildEntity>.Filter.Eq(child => child.Id, childId));

            var update = Builders<TRootEntity>.Update.PullFilter(pathStr,
                Builders<TChildEntity>.Filter.And(filterDefinitions));

            return await _collection.UpdateManyAsync(filterEntity.ToBsonDocument(), update, updateOptions);
        }

        public async Task<UpdateResult> PushAsync(Expression<Func<TRootEntity, bool>> predicate, TChildEntity childEntity, IEnumerable<(string nestedArrayName, string itemId)> path, string arrName)
        {
            UpdateOptions updateOptions = new UpdateOptions();
            (string pathStr, IEnumerable<ArrayFilterDefinition> arrayFilterDefinitions) = GetPathAndArrayFilterDefinitions(path);

            pathStr += arrName;
            updateOptions.ArrayFilters = arrayFilterDefinitions;

            return await _collection.UpdateManyAsync(
                predicate,
                new BsonDocument("$push", new BsonDocument(pathStr, childEntity.ToBsonDocument())),
                updateOptions);
        }

        public async Task<UpdateResult> PushAsync(string rootId, TChildEntity childEntity, IEnumerable<(string nestedArrayName, string itemId)> path, string arrName)
        {
            UpdateOptions updateOptions = new UpdateOptions();
            (string pathStr, IEnumerable<ArrayFilterDefinition> arrayFilterDefinitions) = GetPathAndArrayFilterDefinitions(path);

            pathStr += arrName;
            updateOptions.ArrayFilters = arrayFilterDefinitions;

            return await _collection.UpdateOneAsync(
                root=> root.Id== rootId,
                new BsonDocument("$push", new BsonDocument(pathStr, childEntity.ToBsonDocument())),
                updateOptions);
        }

        public async Task<UpdateResult> PushAsync(TRootEntity filterEntity, TChildEntity childEntity, IEnumerable<(string nestedArrayName, string itemId)> path, string arrName)
        {
            UpdateOptions updateOptions = new UpdateOptions();
            (string pathStr, IEnumerable<ArrayFilterDefinition> arrayFilterDefinitions) = GetPathAndArrayFilterDefinitions(path);

            pathStr += arrName;
            updateOptions.ArrayFilters = arrayFilterDefinitions;

            return await _collection.UpdateManyAsync(
                filterEntity.ToBsonDocument(),
                new BsonDocument("$push", new BsonDocument(pathStr, childEntity.ToBsonDocument())),
                updateOptions);
        }

        public async Task<UpdateResult> SetAsync(Expression<Func<TRootEntity, bool>> predicate, TChildEntity childEntity, IEnumerable<(string nestedArrayName, string itemId)> path)
        {
            UpdateResult updateResult=new UpdateResult.Acknowledged(0,0,null);
            UpdateOptions updateOptions = new UpdateOptions();
            (string pathStr, IEnumerable<ArrayFilterDefinition> arrayFilterDefinitions) = GetPathAndArrayFilterDefinitions(path);

            updateOptions.ArrayFilters = arrayFilterDefinitions;

            foreach (var el in childEntity.ToBsonDocument().Elements)
            {
                updateResult = await _collection.UpdateManyAsync(predicate,new BsonDocument("$set", new BsonDocument(pathStr+ el.Name, el.Value)), updateOptions);
            }

            return updateResult;
        }

        public async Task<UpdateResult> SetAsync(Expression<Func<TRootEntity, bool>> predicate, string fieldName, string fieldNewValue, IEnumerable<(string nestedArrayName, string itemId)> path)
        {
            UpdateOptions updateOptions = new UpdateOptions();
            (string pathStr, IEnumerable<ArrayFilterDefinition> arrayFilterDefinitions) = GetPathAndArrayFilterDefinitions(path);

            updateOptions.ArrayFilters = arrayFilterDefinitions;
            pathStr += pathStr + fieldName;

            return await _collection.UpdateManyAsync(predicate, new BsonDocument("$set", fieldNewValue), updateOptions);
        }

        public async Task<UpdateResult> SetAsync(string rootId, TChildEntity childEntity, IEnumerable<(string nestedArrayName, string itemId)> path)
        {
            UpdateResult updateResult = new UpdateResult.Acknowledged(0, 0, null);
            UpdateOptions updateOptions = new UpdateOptions();
            (string pathStr, IEnumerable<ArrayFilterDefinition> arrayFilterDefinitions) = GetPathAndArrayFilterDefinitions(path);

            updateOptions.ArrayFilters = arrayFilterDefinitions;

            foreach (var el in childEntity.ToBsonDocument().Elements)
            {
                updateResult = await _collection.UpdateOneAsync(root=>root.Id==rootId, new BsonDocument("$set", new BsonDocument(pathStr + el.Name, el.Value)), updateOptions);
            }

            return updateResult;
        }

        public async Task<UpdateResult> SetAsync(string rootId, string fieldName, string fieldNewValue, IEnumerable<(string nestedArrayName, string itemId)> path)
        {
            UpdateOptions updateOptions = new UpdateOptions();
            (string pathStr, IEnumerable<ArrayFilterDefinition> arrayFilterDefinitions) = GetPathAndArrayFilterDefinitions(path);

            updateOptions.ArrayFilters = arrayFilterDefinitions;
            pathStr += pathStr + fieldName;

            return await _collection.UpdateOneAsync(root=>root.Id == rootId, new BsonDocument("$set", fieldNewValue), updateOptions);
        }

        public async Task<UpdateResult> SetAsync(TRootEntity filterEntity, TChildEntity childEntity, IEnumerable<(string nestedArrayName, string itemId)> path)
        {
            UpdateResult updateResult = new UpdateResult.Acknowledged(0, 0, null);
            UpdateOptions updateOptions = new UpdateOptions();
            (string pathStr, IEnumerable<ArrayFilterDefinition> arrayFilterDefinitions) = GetPathAndArrayFilterDefinitions(path);

            updateOptions.ArrayFilters = arrayFilterDefinitions;

            foreach (var el in childEntity.ToBsonDocument().Elements)
            {
                updateResult = await _collection.UpdateManyAsync(filterEntity.ToBsonDocument(), new BsonDocument("$set", new BsonDocument(pathStr + el.Name, el.Value)), updateOptions);
            }

            return updateResult;
        }

        public async Task<UpdateResult> SetAsync(TRootEntity filterEntity, string fieldName, string fieldNewValue, IEnumerable<(string nestedArrayName, string itemId)> path)
        {
            UpdateOptions updateOptions = new UpdateOptions();
            (string pathStr, IEnumerable<ArrayFilterDefinition> arrayFilterDefinitions) = GetPathAndArrayFilterDefinitions(path);

            updateOptions.ArrayFilters = arrayFilterDefinitions;
            pathStr += pathStr + fieldName;

            return await _collection.UpdateManyAsync(filterEntity.ToBsonDocument(), new BsonDocument("$set", fieldNewValue), updateOptions);
        }

        /// <summary>
        /// Create random lowercase string
        /// </summary>
        /// <returns>lowercase string</returns>
        public static string GenerateIndexName(int size, bool isLowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (isLowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }

        /// <summary>
        /// Get path string and List of Array Filter Definitions to describe path across tree.
        /// </summary>
        /// <param name="path">Lists of tuples (array name, id of item we are needed from this array)</param>
        /// <returns>pathStr and arrayFilterDefinitions</returns>
        protected (string pathStr, IEnumerable<ArrayFilterDefinition> arrayFilterDefinitions) GetPathAndArrayFilterDefinitions(IEnumerable<(string nestedArrayName, string itemId)> path)
        {
            List<ArrayFilterDefinition> arrayFilters = new List<ArrayFilterDefinition>();
            string index;
            string pathStr = "";
            foreach (var (nestedArrayKey, itemId) in path)
            {
                do
                {
                    index = GenerateIndexName(5,true);
                } while (pathStr.Contains(index));
                arrayFilters.Add(new BsonDocumentArrayFilterDefinition<TChildEntity>(new BsonDocument($"{index}._id", new ObjectId(itemId))));
                pathStr += $"{nestedArrayKey}.$[{index}].";
            }
            return (pathStr, arrayFilters);
        }
    }
}
