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
            CreateMap<BookDto, Book>().ReverseMap()
                .ForMember(dto => dto.Authors, opt => opt.MapFrom(x => x.BookAuthor.Select(y => y.Author).ToList()))
                .ForMember(dto => dto.Genres, opt => opt.MapFrom(x => x.BookGenre.Select(y => y.Genre).ToList()));
            CreateMap<GenreDto, Genre>().ReverseMap();
            CreateMap<LocationDto, Location>().ReverseMap()
                .ForMember(dto => dto.Rooms, opt => opt.MapFrom(x => x.UserLocation.Select(y => y.RoomNumber)));
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
