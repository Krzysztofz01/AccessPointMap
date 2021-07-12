using AccessPointMap.Service.Dto;
using AccessPointMap.Web.ViewModels;
using AutoMapper;

namespace AccessPointMap.Web.Profiles
{
    public class AccessPointStatisticsProfile : Profile
    {
        public AccessPointStatisticsProfile()
        {
            CreateMap<AccessPointStatisticsDto, AccessPointStatisticsGetView>().ReverseMap();
        }
    }
}
