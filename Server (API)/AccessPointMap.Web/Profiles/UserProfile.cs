﻿using AccessPointMap.Service.Dto;
using AccessPointMap.Web.ViewModels;
using AutoMapper;

namespace AccessPointMap.Web.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserDto, UserGetView>().ReverseMap();

            CreateMap<UserDto, UserCurrentGetView>().ReverseMap();

            CreateMap<UserDto, UserPatchView>().ReverseMap();

            CreateMap<UserDto, UserGetViewPublic>().ReverseMap();
        }
    }
}
