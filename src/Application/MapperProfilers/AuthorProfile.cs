using Application.Dto;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using RdbmsEntities = Domain.RDBMS.Entities;
namespace Application.MapperProfilers
{
    public class AuthorProfile : Profile
    {
        public AuthorProfile()
        {
            CreateMap<AuthorDto, RdbmsEntities.Author>().ReverseMap();
            CreateMap<AuthorDto, RdbmsEntities.BookAuthor>()
                .ForMember(a => a.AuthorId, opt => opt.MapFrom(dto => dto.Id));
        }
    }
}
