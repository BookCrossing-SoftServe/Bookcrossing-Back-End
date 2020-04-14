using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Domain.NoSQL.Entities
{
    [BsonIgnoreExtraElements]
    public class BookRootComment : IRootEntityBase
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonIgnoreIfNull]
        public string Id { get; set; }
        [BsonIgnoreIfNull]
        public string Text { get; set; }
        [BsonIgnoreIfNull]
        public string Date { get; set; }
        [BsonIgnoreIfDefault]
        public int BookId { get; set; }
        [BsonIgnoreIfDefault]
        public int UserId { get; set; }
        [BsonIgnoreIfNull]
        public IEnumerable<BookChildComment> Comments { get; set; }
        public BookRootComment() { }
        public BookRootComment(bool IsForInserting)
        {
            if (IsForInserting)
            {
                Comments = new List<BookChildComment>();
            }
        }
    }
}
