using System.Linq;
using Application.Dto;
using AutoMapper;
using RdbmsEntities = Domain.RDBMS.Entities;
using NoSqlEntities = Domain.NoSQL.Entities;

namespace Application
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<UserUpdateDto, RdbmsEntities.User>().ReverseMap();
            CreateMap<AuthorDto, RdbmsEntities.Author>().ReverseMap();
            CreateMap<NewAuthorDto, RdbmsEntities.Author>().ReverseMap();
            CreateMap<AuthorDto, RdbmsEntities.BookAuthor>()
                .ForMember(a => a.AuthorId, opt => opt.MapFrom(dto => dto.Id))
                .ForMember(a => a.Author, opt =>
                {
                    opt.Condition(dto => dto.Id == 0);
                    opt.MapFrom(dto => dto);
                });
            CreateMap<GenreDto, RdbmsEntities.BookGenre>()
                .ForMember(a => a.GenreId, opt => opt.MapFrom(dto => dto.Id))
                .ForMember(a => a.Genre, opt =>
                {
                    opt.Condition(dto => dto.Id == 0);
                    opt.MapFrom(dto => dto);
                });
            CreateMap<BookDto, RdbmsEntities.Book>()
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
            CreateMap<RdbmsEntities.Book, BookDto>()
                .ForMember(dto => dto.Authors, opt => opt.MapFrom(x => x.BookAuthor.Select(y => y.Author).ToList()))
                .ForMember(dto => dto.Genres, opt => opt.MapFrom(x => x.BookGenre.Select(y => y.Genre).ToList()));
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
        }
    }
}
