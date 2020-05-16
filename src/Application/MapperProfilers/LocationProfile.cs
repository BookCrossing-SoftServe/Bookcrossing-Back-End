using Application.Dto;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RdbmsEntities = Domain.RDBMS.Entities;
namespace Application.MapperProfilers
{
    public class LocationProfile : Profile
    {
        public LocationProfile()
        {
            CreateMap<LocationDto, RdbmsEntities.Location>().ReverseMap()
               .ForMember(dto => dto.Rooms, opt => opt.MapFrom(x => x.UserLocation.Select(y => y.RoomNumber)))
               .ForMember(a => a.Id, opt => opt.Condition(a => a.Id != 0));
            CreateMap<RoomLocationDto, RdbmsEntities.UserRoom>().ReverseMap();
        }
    }
}
