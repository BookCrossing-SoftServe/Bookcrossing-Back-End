using System;
using System.Collections.Generic;
using Domain.RDBMS.Entities;

namespace Application.Dto
{
    public class BookGetDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
        public string Publisher { get; set; }
        public BookState State { get; set; }
        public DateTime DateAdded { get; set; }
        public string Notice { get; set; }
        public string ImagePath { get; set; }
        public double Rating { get; set; }
        public int LanguageId { get; set; }
        public List<AuthorDto> Authors { get; set; }
        public List<GenreDto> Genres { get; set; }
        public RoomLocationDto Location { get; set; }
    }
}
