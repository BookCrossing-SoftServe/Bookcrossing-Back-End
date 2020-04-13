using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Domain.NoSQL.Entities
{
    [BsonIgnoreExtraElements]
    public class BookChildComment : IChildEntityBase
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
        public int CommentOwnerId { get; set; }
        [BsonIgnoreIfNull]
        public IEnumerable<BookChildComment> Comments { get; set; }
        public BookChildComment() {}
        public BookChildComment(bool IsForInserting)
        {
            if (IsForInserting)
            {
                Id = ObjectId.GenerateNewId().ToString();
                Comments = new List<BookChildComment>();
            }
        }
    }
}
