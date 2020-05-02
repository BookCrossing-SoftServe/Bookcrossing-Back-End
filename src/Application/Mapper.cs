using System.Linq;
using Application.Dto;
using AutoMapper;
using RdbmsEntities = Domain.RDBMS.Entities;
using NoSqlEntities = Domain.NoSQL.Entities;
using System;

namespace Application
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<NoSqlEntities.BookChildComment, Dto.Comment.Book.ChildDto>()
                .ForMember(dto => dto.Date, opt => opt.MapFrom(entity => Convert.ToDateTime(entity.Date).ToLocalTime()))
                .ForMember(dto => dto.Comments, opt => opt.MapFrom(entity => entity.Comments))
                .ForMember(dto => dto.Owner, opt => opt.MapFrom(entity => new Dto.Comment.OwnerDto() { Id = entity.OwnerId }));
            CreateMap<NoSqlEntities.BookRootComment, Dto.Comment.Book.RootDto>()
                .ForMember(dto => dto.Date, opt => opt.MapFrom(entity => Convert.ToDateTime(entity.Date).ToLocalTime()))
                .ForMember(dto => dto.Comments, opt => opt.MapFrom(entity => entity.Comments))
                .ForMember(dto => dto.Owner, opt => opt.MapFrom(entity => new Dto.Comment.OwnerDto() { Id = entity.OwnerId }));
            CreateMap<RdbmsEntities.User, Dto.Comment.OwnerDto>()
                .ForMember(dto => dto.Role, opt => opt.MapFrom(entity => entity.Role.Name));
            CreateMap<UserUpdateDto, RdbmsEntities.User>().ReverseMap();
            CreateMap<AuthorDto, RdbmsEntities.Author>().ReverseMap();
            CreateMap<AuthorDto, RdbmsEntities.BookAuthor>()
                .ForMember(a => a.AuthorId, opt => opt.MapFrom(dto => dto.Id));
            CreateMap<GenreDto, RdbmsEntities.BookGenre>()
                .ForMember(a => a.GenreId, opt => opt.MapFrom(dto => dto.Id));
            CreateMap<BookPutDto, RdbmsEntities.Book>()
                .ForMember(entity => entity.BookAuthor, opt => opt.MapFrom(x => x.Authors))
                .ForMember(entity => entity.BookGenre, opt => opt.MapFrom(x => x.Genres))
                .AfterMap((model, entity) =>
                    {
                        foreach (var item in entity.BookAuthor)
                        {
                            item.BookId = entity.Id;
                        }
                        foreach (var item in entity.BookGenre)
                        {
                            item.BookId = entity.Id;
                        }
                    });
            CreateMap<RdbmsEntities.Book, BookPutDto>()
                .ForMember(dto => dto.Authors, opt => opt.MapFrom(x => x.BookAuthor.Select(y => y.Author).ToList()))
                .ForMember(dto => dto.Genres, opt => opt.MapFrom(x => x.BookGenre.Select(y => y.Genre).ToList()));
            CreateMap<RdbmsEntities.Book, BookGetDto>()
               .ForMember(dto => dto.Authors, opt => opt.MapFrom(x => x.BookAuthor.Select(y => y.Author).ToList()))
               .ForMember(dto => dto.Genres, opt => opt.MapFrom(x => x.BookGenre.Select(y => y.Genre).ToList()))
               .ForMember(dto => dto.Locations, opt => opt.MapFrom(x => x.User.UserLocation));
            CreateMap<GenreDto, RdbmsEntities.Genre>().ReverseMap();
            CreateMap<LocationDto, RdbmsEntities.Location>().ReverseMap()
                .ForMember(dto => dto.Rooms, opt => opt.MapFrom(x => x.UserLocation.Select(y => y.RoomNumber)))
                .ForMember(a => a.Id, opt => opt.Condition(a => a.Id != 0));
            CreateMap<RoomLocationDto, RdbmsEntities.UserLocation>().ReverseMap();
            CreateMap<LoginDto, RdbmsEntities.User>().ReverseMap();
            CreateMap<RegisterDto, RdbmsEntities.User>().ReverseMap();
            CreateMap<RequestDto, RdbmsEntities.Request>().ReverseMap()
                .ForMember(a => a.Id, opt => opt.Condition(a => a.Id != 0))
                .ForMember(dto => dto.Book, opt => opt.MapFrom(x => x.Book))
                .ForMember(dto => dto.Owner, opt => opt.MapFrom(x => x.Owner))
                .ForMember(dto => dto.User, opt => opt.MapFrom(x => x.User));
            CreateMap<RdbmsEntities.User, UserDto>().ReverseMap();
            CreateMap<UserProfileDto, RdbmsEntities.User>().ForMember(x => x.Book, opt => opt.MapFrom(x => x.Books))
                .ReverseMap();
            CreateMap<BookPostDto, RdbmsEntities.Book>()
                .ForMember(entity => entity.BookAuthor, opt => opt.MapFrom(x => x.Authors))
                .ForMember(entity => entity.BookGenre, opt => opt.MapFrom(x => x.Genres))
                .AfterMap((model, entity) =>
                {
                    foreach (var item in entity.BookAuthor)
                    {
                        item.BookId = entity.Id;
                    }
                    foreach (var item in entity.BookGenre)
                    {
                        item.BookId = entity.Id;
                    }
                });
            CreateMap<UserDto, RdbmsEntities.User>().ReverseMap()
                .ForMember(a => a.Id, opt => opt.Condition(a => a.Id != 0))
                .ForMember(dto => dto.UserLocation, opt => opt.MapFrom(x => x.UserLocation))
                .ForMember(dto => dto.Role, opt => opt.MapFrom(x => x.Role));
        }
    }
}
