using System.Collections.Generic;

namespace Application.Dto.Comment
{
    public class BookCommentInsertDto
    {
        public IEnumerable<string> Ids { get; set; }
        public string Text { get; set; }
        public int BookId { get; set; }
        public int CommentOwnerId { get; set; }
    }
}