using Application.Dto;
using AutoMapper;
using Domain.RDBMS.Entities;

namespace Application.MapperProfilers
{
    public class AphorismProfile : Profile
    {
        public AphorismProfile()
        {
            CreateMap<Aphorism, AphorismDto>().ReverseMap();
        }
    }
}
