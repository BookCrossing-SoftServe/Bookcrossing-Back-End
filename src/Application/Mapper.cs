using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Dto;
using AutoMapper;
using Domain.Entities;

namespace Application
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<AuthorDto, Author>().ReverseMap().ForMember(a => a.Id, opt => opt.Condition(a => a.Id != 0));
            CreateMap<AuthorDto, BookAuthor>().ReverseMap();
            CreateMap<GenreDto, BookGenre>().ReverseMap();
            //CreateMap<BookDto, Book>()
            //    .ForMember(entity => entity.BookAuthor.Select(b => b.Author), opt => opt.MapFrom(x => x.Authors))
            //    .ForMember(entity => entity.BookGenre.Select(b => b.Genre), opt => opt.MapFrom(x => x.Genres));
            CreateMap<Book, BookDto>()
                .ForMember(dto => dto.Authors, opt => opt.MapFrom(x => x.BookAuthor.Select(y => y.Author).ToList()))
                .ForMember(dto => dto.Genres, opt => opt.MapFrom(x => x.BookGenre.Select(y => y.Genre).ToList())).ReverseMap();
            CreateMap<GenreDto, Genre>().ReverseMap();
            CreateMap<LocationDto, Location>().ReverseMap();
            CreateMap<RoomLocationDto, UserLocation>().ReverseMap();
            CreateMap<LoginDto, User>().ReverseMap();
            CreateMap<RegisterDto, User>().ReverseMap();
            CreateMap<RequestDto, Request>().ReverseMap().ForMember(a => a.Id, opt => opt.Condition(a => a.Id != 0));
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<UserProfileDto, User>().ForMember(x => x.Book, opt => opt.MapFrom(x => x.Books))
                .ReverseMap();
        }
    }
}
