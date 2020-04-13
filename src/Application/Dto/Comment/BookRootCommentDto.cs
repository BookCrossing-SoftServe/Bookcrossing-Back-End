using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace Application.Dto.Comment
{
    public  class BookRootCommentDto
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public int BookId { get; set; }
        public CommentOwnerDto CommentOwner { get; set; }
        public IEnumerable<BookChildCommentDto> Comments { get; set; }
        public BookRootCommentDto() {}
        public BookRootCommentDto(bool IsForInserting)
        {
            if (IsForInserting)
            {
                Id = ObjectId.GenerateNewId().ToString();
                Comments = new List<BookChildCommentDto>();            
            }
        }
    }
}
