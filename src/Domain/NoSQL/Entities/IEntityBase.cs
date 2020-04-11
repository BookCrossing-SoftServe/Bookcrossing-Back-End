using MongoDB.Bson;

namespace Domain.NoSQL.Entities
{
    public interface IEntityBase
    {
        ObjectId Id { get; set; }
    }
}
