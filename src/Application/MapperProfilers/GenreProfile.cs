using Application.Dto;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using RdbmsEntities = Domain.RDBMS.Entities;
namespace Application.MapperProfilers
{
    public class GenreProfile : Profile
    {
        public GenreProfile()
        {
            CreateMap<GenreDto, RdbmsEntities.BookGenre>()
               .ForMember(a => a.GenreId, opt => opt.MapFrom(dto => dto.Id));

            CreateMap<GenreDto, RdbmsEntities.Genre>().ReverseMap();
        }
    }
}
