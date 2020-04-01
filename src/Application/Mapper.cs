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
            CreateMap<AuthorDto, BookAuthor>()
                .ForMember(a => a.AuthorId, opt => opt.MapFrom(dto => dto.Id))
                .ForMember(a => a.Author, opt =>
                {
                    opt.Condition(dto => dto.Id == 0);
                    opt.MapFrom(dto => dto);
                });
            CreateMap<GenreDto, BookGenre>()
                .ForMember(a => a.GenreId, opt => opt.MapFrom(dto => dto.Id))
                .ForMember(a => a.Genre, opt =>
                {
                    opt.Condition(dto => dto.Id == 0);
                    opt.MapFrom(dto => dto);
                });
            CreateMap<BookDto, Book>()
                .ForMember(entity => entity.BookAuthor, opt => opt.MapFrom(x => x.Authors))
                .ForMember(entity => entity.BookGenre, opt => opt.MapFrom(x => x.Genres))
                .AfterMap((model, entity) =>
                    {
                        foreach (var item in entity.BookAuthor)
                        {
                            item.Book = entity;
                            item.BookId = entity.Id;
                        }
                        foreach (var item in entity.BookGenre)
                        {
                            item.Book = entity;
                            item.BookId = entity.Id;
                        }
                    }); 
            CreateMap<Book, BookDto>()
                .ForMember(dto => dto.Authors, opt => opt.MapFrom(x => x.BookAuthor.Select(y => y.Author).ToList()))
                .ForMember(dto => dto.Genres, opt => opt.MapFrom(x => x.BookGenre.Select(y => y.Genre).ToList()));
            CreateMap<GenreDto, Genre>().ReverseMap();
            CreateMap<LocationDto, Location>().ReverseMap()
                .ForMember(dto => dto.Rooms, opt => opt.MapFrom(x => x.UserLocation.Select(y => y.RoomNumber)))
                .ForMember(a => a.Id, opt => opt.Condition(a => a.Id != 0));
            CreateMap<RoomLocationDto, UserLocation>().ReverseMap();
            CreateMap<LoginDto, User>().ReverseMap();
            CreateMap<RegisterDto, User>().ReverseMap();
            CreateMap<RequestDto, Request>().ReverseMap()
                .ForMember(a => a.Id, opt => opt.Condition(a => a.Id != 0))
                .ForMember(dto => dto.Book, opt => opt.MapFrom(x => x.Book))
                .ForMember(dto => dto.Owner, opt => opt.MapFrom(x => x.Owner))
                .ForMember(dto => dto.User, opt => opt.MapFrom(x => x.User));
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<UserProfileDto, User>().ForMember(x => x.Book, opt => opt.MapFrom(x => x.Books))
                .ReverseMap();
        }
    }
}
