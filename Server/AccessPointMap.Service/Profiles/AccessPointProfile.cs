using AccessPointMap.Domain;
using AccessPointMap.Service.Dto;
using AutoMapper;

namespace AccessPointMap.Service.Profiles
{
    public class AccessPointProfile : Profile
    {
        public AccessPointProfile()
        {
            CreateMap<AccessPoint, AccessPointDto>().ReverseMap();
        }
    }
}
