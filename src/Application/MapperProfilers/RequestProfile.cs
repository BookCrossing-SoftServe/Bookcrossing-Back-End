using Application.Dto;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using RdbmsEntities = Domain.RDBMS.Entities;
namespace Application.MapperProfilers
{
    public class RequestProfile : Profile
    {
        public RequestProfile()
        {
            CreateMap<RequestDto, RdbmsEntities.Request>().ReverseMap()
                .ForMember(a => a.Id, opt => opt.Condition(a => a.Id != 0))
                .ForMember(dto => dto.Book, opt => opt.MapFrom(x => x.Book))
                .ForPath(dto => dto.Book.Location, opt => opt.MapFrom(x => x.Owner.UserRoom))
                .ForMember(dto => dto.Owner, opt => opt.MapFrom(x => x.Owner))
                .ForMember(dto => dto.User, opt => opt.MapFrom(x => x.User));
        }
    }
}
