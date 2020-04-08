using Domain.NoSQL;

namespace Infrastructure.NoSQL
{
    public class MongoSettings : IMongoSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
