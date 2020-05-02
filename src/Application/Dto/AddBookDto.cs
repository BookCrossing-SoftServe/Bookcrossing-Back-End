using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Application.Dto
{
    public class AddBookDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
        public string Publisher { get; set; }
        public bool Available { get; set; }
        public string Notice { get; set; }
        public IFormFile Image { get; set; }

        public List<AuthorDto> Authors { get; set; }
        public List<GenreDto> Genres { get; set; }
    }
}
