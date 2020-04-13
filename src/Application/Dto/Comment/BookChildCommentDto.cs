using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace Application.Dto.Comment
{
    public class BookChildCommentDto
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public CommentOwnerDto CommentOwner { get; set; }
        public IEnumerable<BookChildCommentDto> Comments { get; set; }
        public BookChildCommentDto() {}
        public BookChildCommentDto(bool IsForInserting)
        {
            if (IsForInserting)
            {
                Id = ObjectId.GenerateNewId().ToString();
                Comments = new List<BookChildCommentDto>();
            }
        }
    }
}
