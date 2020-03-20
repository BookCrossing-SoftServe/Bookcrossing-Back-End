using System;
using System.Collections.Generic;
using System.Text;
using Application.Dto;
using AutoMapper;
using Domain.Entities;

namespace Application
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<AuthorDto, Author>().ReverseMap();
            CreateMap<BookDto, Book>().ReverseMap();
            CreateMap<GenreDto, Genre>().ReverseMap();
            CreateMap<LocationDto, Location>().ReverseMap();
            CreateMap<LoginDto, User>().ReverseMap();
            CreateMap<RegisterDto, User>().ReverseMap();
            CreateMap<RequestDto, Request>().ReverseMap();
            CreateMap<User, UserDto>()
                //.ForMember(m => m.UserLocation, opt => opt.MapFrom(x => x.UserLocation))
                .ReverseMap();
            CreateMap<UserProfileDto, User>().ReverseMap();

                
        }
    }
}
