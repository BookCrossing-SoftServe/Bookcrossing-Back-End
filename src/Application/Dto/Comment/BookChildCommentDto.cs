using System;
using System.Collections.Generic;

namespace Application.Dto.Comment
{
    public class BookChildCommentDto
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public int CommentOwnerId { get; set; }
        public IEnumerable<BookChildCommentDto> Comments { get; set; }
        public BookChildCommentDto() {}
    }
}
