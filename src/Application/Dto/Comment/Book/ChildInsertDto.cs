using System.Collections.Generic;

namespace Application.Dto.Comment.Book
{
    public class ChildInsertDto
    {
        public IEnumerable<string> Ids { get; set; }
        public string Text { get; set; }
        public int CommentOwnerId { get; set; }
    }
}