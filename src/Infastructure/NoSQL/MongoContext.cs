using Domain.NoSQL;
using Domain.NoSQL.Entities;
using MongoDB.Driver;
using System;

namespace Infrastructure.NoSQL
{
    public class MongoContext
    {
        private IMongoDatabase _database = null;

        public MongoContext(IMongoSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            if (client != null)
            {
                _database = client.GetDatabase(settings.DatabaseName);
            }
        }

        public IMongoCollection<TEntity> Collection<TEntity>() where TEntity : IEntityBase => typeof(TEntity).Name switch
        {
            nameof(BookRootComment) => _database.GetCollection<TEntity>("BookComments"),
            //nameof(ProfileRootComment) => _database.GetCollection<TEntity>("ProfileComments"),
            _ => throw new Exception()
        };
    }
}
