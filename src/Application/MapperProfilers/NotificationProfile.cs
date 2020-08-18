using System;
using System.Collections.Generic;
using System.Text;
using Application.Dto;
using AutoMapper;
using Domain.RDBMS.Entities;

namespace Application.MapperProfilers
{
    public class NotificationProfile: Profile
    {
        public NotificationProfile()
        {
            CreateMap<Notification, NotificationDto>()
                .ForMember(
                    dto => dto.IsSeen, 
                    config => config.MapFrom(entity => entity.IsRead))
                .ForMember(
                    dto => dto.Date, 
                    config => config.MapFrom(entity => entity.CreatedAt));
        }
    }
}
