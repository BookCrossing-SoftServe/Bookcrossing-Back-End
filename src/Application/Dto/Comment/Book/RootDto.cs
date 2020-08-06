using System;
using System.Collections.Generic;

namespace Application.Dto.Comment.Book
{
    public  class RootDto
    {
        public string Id { get; set; }

        public string Text { get; set; }

        public DateTime Date { get; set; }

        public int Rating { get; set; }

        public int BookId { get; set; }

        public bool IsDeleted { get; set; }

        public OwnerDto Owner { get; set; }

        public IEnumerable<ChildDto> Comments { get; set; }
    }
}
