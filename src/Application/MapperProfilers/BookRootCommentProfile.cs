using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using NoSqlEntities = Domain.NoSQL.Entities;
namespace Application.MapperProfilers
{
    public class BookRootCommentProfile : Profile
    {
        public BookRootCommentProfile()
        {
            CreateMap<NoSqlEntities.BookRootComment, Dto.Comment.Book.RootDto>()
                .ForMember(dto => dto.Rating, opt => opt.MapFrom(entity => entity.Rating))
             .ForMember(dto => dto.Date, opt => opt.MapFrom(entity => Convert.ToDateTime(entity.Date).ToLocalTime()))
             .ForMember(dto => dto.Comments, opt => opt.MapFrom(entity => entity.Comments))
             .ForMember(dto => dto.Owner, opt => opt.MapFrom(entity => new Dto.Comment.OwnerDto() { Id = entity.OwnerId }));
        }
    }
}
