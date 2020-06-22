using Application.Dto;
using AutoMapper;
using Domain.RDBMS.Entities;

namespace Application.MapperProfilers
{
    public class LanguageProfile : Profile
    {
        public LanguageProfile()
        {
            CreateMap<Language, LanguageDto>().ReverseMap();
        }
    }
}
