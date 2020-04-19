using System;
using System.Collections.Generic;

namespace Application.Dto.Comment.Book
{
    public  class RootDto
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public int BookId { get; set; }
        public int CommentOwnerId { get; set; }
        public IEnumerable<ChildDto> Comments { get; set; }
        public RootDto() {}
    }
}
