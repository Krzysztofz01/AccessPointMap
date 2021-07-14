using AccessPointMap.Service.Dto;
using AccessPointMap.Web.ViewModels;
using AutoMapper;

namespace AccessPointMap.Web.Profiles
{
    public class AccessPointProfile : Profile
    {
        public AccessPointProfile()
        {
            CreateMap<AccessPointDto, AccessPointGetView>().ReverseMap();

            CreateMap<AccessPointDto, AccessPointPostView>().ReverseMap();

            CreateMap<AccessPointDto, AccessPointPatchView>().ReverseMap();

            CreateMap<AccessPointDto, AccessPointGetViewPublic>().ReverseMap();
        }
    }
}
