using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Domain.RDBMS.Entities;

namespace Application.Dto
{
    public class BookPutDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
        public string Publisher { get; set; }
        public BookState State { get; set; }
        public string Notice { get; set; }
        public IFormFile Image { get; set; }
        public List<string> FieldMasks { get; set; }
        public int LanguageId { get; set; }
        public LanguageDto Language { get; set; }
        public List<AuthorDto> BookAuthor { get; set; }
        public List<GenreDto> bookGenre { get; set; }
    }
}
