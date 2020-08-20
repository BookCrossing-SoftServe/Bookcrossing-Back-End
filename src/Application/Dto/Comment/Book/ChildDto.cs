using System;
using System.Collections.Generic;

namespace Application.Dto.Comment.Book
{
    public class ChildDto
    {
        public string Id { get; set; }

        public string Text { get; set; }

        public DateTime Date { get; set; }

        public OwnerDto Owner { get; set; }

        public bool IsDeleted { get; set; }

        public IEnumerable<ChildDto> Comments { get; set; }
    }
}
