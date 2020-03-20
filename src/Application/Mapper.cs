using System;
using System.Collections.Generic;
using System.Text;
using Application.Dto;
using AutoMapper;
using Domain.Entities;

namespace Application
{
    class Mapper : Profile
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
            CreateMap<UserDto, User>().ReverseMap();
            CreateMap<UserProfileDto, User>().ReverseMap();




        }
    }
}
