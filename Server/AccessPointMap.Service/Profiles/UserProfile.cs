using AccessPointMap.Domain;
using AccessPointMap.Service.Dto;
using AutoMapper;

namespace AccessPointMap.Service.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}
