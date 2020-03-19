using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dto
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
        public string Publisher { get; set; }
        public bool Available { get; set; }

        public List<AuthorDto> Authors { get; set; }
        public List<GenreDto> Genres { get; set; }
    }
}
