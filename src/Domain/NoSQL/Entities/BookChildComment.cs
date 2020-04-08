using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Domain.NoSQL.Entities
{
    [BsonIgnoreExtraElements]
    public class BookChildComment
    {
        [BsonIgnoreIfDefault]
        public ObjectId Id { get; set; }
        [BsonIgnoreIfNull]
        public string Text { get; set; }
        [BsonIgnoreIfNull]
        public string Date { get; set; }
        [BsonIgnoreIfDefault]
        public int UserId { get; set; }
        [BsonIgnoreIfNull]
        public IEnumerable<BookChildComment> Comments { get; set; }
    }
}
