using Application.Dto;
using AutoMapper;
using Domain.RDBMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RdbmsEntities = Domain.RDBMS.Entities;

namespace Application.MapperProfilers
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<BookPutDto, RdbmsEntities.Book>()
                .ForMember(entity => entity.BookAuthor, opt => opt.MapFrom(x => x.BookAuthor))
                .ForMember(entity => entity.BookGenre, opt => opt.MapFrom(x => x.bookGenre))
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
                .ForMember(dto => dto.BookAuthor, opt => opt.MapFrom(x => x.BookAuthor.Select(y => y.Author).ToList()))
                .ForMember(dto => dto.bookGenre, opt => opt.MapFrom(x => x.BookGenre.Select(y => y.Genre).ToList()));

            CreateMap<RdbmsEntities.Book, BookGetDto>()
               .ForMember(dto => dto.Authors, opt => opt.MapFrom(x => x.BookAuthor.Select(y => y.Author).ToList()))
               .ForMember(dto => dto.Genres, opt => opt.MapFrom(x => x.BookGenre.Select(y => y.Genre).ToList()))
               .ForMember(dto => dto.Locations, opt => opt.MapFrom(x => x.User.UserRoom));

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
        }
    }
}
