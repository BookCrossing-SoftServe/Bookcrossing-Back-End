using System.Collections.Generic;

namespace Application.Dto.Comment.Book
{
    public class ChildUpdateDto
    {
        public IEnumerable<string> Ids { get; set; }
        public string Text { get; set; }
    }
}
