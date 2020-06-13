using Application.Dto;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using RdbmsEntities = Domain.RDBMS.Entities;
namespace Application.MapperProfilers
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RdbmsEntities.User, Dto.Comment.OwnerDto>()
             .ForMember(dto => dto.Role, opt => opt.MapFrom(entity => entity.Role.Name));

            CreateMap<UserUpdateDto, RdbmsEntities.User>().ReverseMap();

            CreateMap<UpdatedUserDto, RdbmsEntities.User>().ReverseMap();

            CreateMap<LoginDto, RdbmsEntities.User>().ReverseMap();

            CreateMap<RegisterDto, RdbmsEntities.User>().ReverseMap();

            CreateMap<RdbmsEntities.User, UserDto>().ReverseMap();

            CreateMap<UserProfileDto, RdbmsEntities.User>().ForMember(x => x.Book, opt => opt.MapFrom(x => x.Books))
                .ReverseMap();

            CreateMap<UserDto, RdbmsEntities.User>().ReverseMap()
                .ForMember(a => a.Id, opt => opt.Condition(a => a.Id != 0))
                .ForMember(dto => dto.UserLocation, opt => opt.MapFrom(x => x.UserRoom))
                .ForMember(dto => dto.Role, opt => opt.MapFrom(x => x.Role));
        }
    }

}
