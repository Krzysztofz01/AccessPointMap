using AccessPointMap.Domain.AccessPoints;
using AccessPointMap.Domain.AccessPoints.AccessPointStamps;
using AutoMapper;
using static AccessPointMap.Application.AccessPoints.Dto;

namespace AccessPointMap.Application.AccessPoints
{
    public class AccessPointMapperProfile : Profile
    {
        public AccessPointMapperProfile()
        {
            CreateMap<AccessPoint, AccessPointSimple>()
                .ForMember(d => d.Id, m => m.MapFrom(s => s.Id))
                .ForMember(d => d.Bssid, m => m.MapFrom(s => s.Bssid.Value))
                .ForMember(d => d.Ssid, m => m.MapFrom(s => s.Ssid.Value))
                .ForMember(d => d.Frequency, m => m.MapFrom(s => s.Frequency.Value))
                .ForMember(d => d.DeviceType, m => m.MapFrom(s => s.DeviceType.Value))
                .ForMember(d => d.HighSignalLatitude, m => m.MapFrom(s => s.Positioning.HighSignalLatitude))
                .ForMember(d => d.HighSignalLongitude, m => m.MapFrom(s => s.Positioning.HighSignalLongitude))
                .ForMember(d => d.SignalArea, m => m.MapFrom(s => s.Positioning.SignalArea))
                .ForMember(d => d.SerializedSecurityPayload, m => m.MapFrom(s => s.Security.SerializedSecurityPayload))
                .ForMember(d => d.IsSecure, m => m.MapFrom(s => s.Security.IsSecure));

            CreateMap<AccessPoint, AccessPointDetails>()
                .ForMember(d => d.Id, m => m.MapFrom(s => s.Id))
                .ForMember(d => d.Bssid, m => m.MapFrom(s => s.Bssid.Value))
                .ForMember(d => d.Manufacturer, m => m.MapFrom(s => s.Manufacturer.Value))
                .ForMember(d => d.Ssid, m => m.MapFrom(s => s.Ssid.Value))
                .ForMember(d => d.Frequency, m => m.MapFrom(s => s.Frequency.Value))
                .ForMember(d => d.DeviceType, m => m.MapFrom(s => s.DeviceType.Value))
                .ForMember(d => d.ContributorId, m => m.MapFrom(s => s.ContributorId.Value))
                .ForMember(d => d.CreationTimestamp, m => m.MapFrom(s => s.CreationTimestamp.Value))
                .ForMember(d => d.VersionTimestamp, m => m.MapFrom(s => s.VersionTimestamp.Value))
                .ForMember(d => d.LowSignalLevel, m => m.MapFrom(s => s.Positioning.LowSignalLevel))
                .ForMember(d => d.LowSignalLatitude, m => m.MapFrom(s => s.Positioning.LowSignalLatitude))
                .ForMember(d => d.LowSignalLongitude, m => m.MapFrom(s => s.Positioning.LowSignalLongitude))
                .ForMember(d => d.HighSignalLevel, m => m.MapFrom(s => s.Positioning.HighSignalLevel))
                .ForMember(d => d.HighSignalLatitude, m => m.MapFrom(s => s.Positioning.HighSignalLatitude))
                .ForMember(d => d.HighSignalLongitude, m => m.MapFrom(s => s.Positioning.HighSignalLongitude))
                .ForMember(d => d.SignalRadius, m => m.MapFrom(s => s.Positioning.SignalRadius))
                .ForMember(d => d.SignalArea, m => m.MapFrom(s => s.Positioning.SignalArea))
                .ForMember(d => d.RawSecurityPayload, m => m.MapFrom(s => s.Security.RawSecurityPayload))
                .ForMember(d => d.SerializedSecurityPayload, m => m.MapFrom(s => s.Security.SerializedSecurityPayload))
                .ForMember(d => d.IsSecure, m => m.MapFrom(s => s.Security.IsSecure))
                .ForMember(d => d.Stamps, m => m.MapFrom(s => s.Stamps));

            CreateMap<AccessPoint, AccessPointDetailsAdministration>()
                .ForMember(d => d.Id, m => m.MapFrom(s => s.Id))
                .ForMember(d => d.Bssid, m => m.MapFrom(s => s.Bssid.Value))
                .ForMember(d => d.Manufacturer, m => m.MapFrom(s => s.Manufacturer.Value))
                .ForMember(d => d.Ssid, m => m.MapFrom(s => s.Ssid.Value))
                .ForMember(d => d.Frequency, m => m.MapFrom(s => s.Frequency.Value))
                .ForMember(d => d.DeviceType, m => m.MapFrom(s => s.DeviceType.Value))
                .ForMember(d => d.ContributorId, m => m.MapFrom(s => s.ContributorId.Value))
                .ForMember(d => d.CreationTimestamp, m => m.MapFrom(s => s.CreationTimestamp.Value))
                .ForMember(d => d.VersionTimestamp, m => m.MapFrom(s => s.VersionTimestamp.Value))
                .ForMember(d => d.LowSignalLevel, m => m.MapFrom(s => s.Positioning.LowSignalLevel))
                .ForMember(d => d.LowSignalLatitude, m => m.MapFrom(s => s.Positioning.LowSignalLatitude))
                .ForMember(d => d.LowSignalLongitude, m => m.MapFrom(s => s.Positioning.LowSignalLongitude))
                .ForMember(d => d.HighSignalLevel, m => m.MapFrom(s => s.Positioning.HighSignalLevel))
                .ForMember(d => d.HighSignalLatitude, m => m.MapFrom(s => s.Positioning.HighSignalLatitude))
                .ForMember(d => d.HighSignalLongitude, m => m.MapFrom(s => s.Positioning.HighSignalLongitude))
                .ForMember(d => d.SignalRadius, m => m.MapFrom(s => s.Positioning.SignalRadius))
                .ForMember(d => d.SignalArea, m => m.MapFrom(s => s.Positioning.SignalArea))
                .ForMember(d => d.RawSecurityPayload, m => m.MapFrom(s => s.Security.RawSecurityPayload))
                .ForMember(d => d.SerializedSecurityPayload, m => m.MapFrom(s => s.Security.SerializedSecurityPayload))
                .ForMember(d => d.IsSecure, m => m.MapFrom(s => s.Security.IsSecure))
                .ForMember(d => d.Note, m => m.MapFrom(s => s.Note))
                .ForMember(d => d.DisplayStatus, m => m.MapFrom(s => s.DisplayStatus))
                .ForMember(d => d.Stamps, m => m.MapFrom(s => s.Stamps));

            CreateMap<AccessPointStamp, AccessPointStampDetails>()
                .ForMember(d => d.Id, m => m.MapFrom(s => s.Id))
                .ForMember(d => d.Ssid, m => m.MapFrom(s => s.Ssid.Value))
                .ForMember(d => d.Frequency, m => m.MapFrom(s => s.Frequency.Value))
                .ForMember(d => d.DeviceType, m => m.MapFrom(s => s.DeviceType.Value))
                .ForMember(d => d.ContributorId, m => m.MapFrom(s => s.ContributorId.Value))
                .ForMember(d => d.CreationTimestamp, m => m.MapFrom(s => s.CreationTimestamp.Value))
                .ForMember(d => d.LowSignalLevel, m => m.MapFrom(s => s.Positioning.LowSignalLevel))
                .ForMember(d => d.LowSignalLatitude, m => m.MapFrom(s => s.Positioning.LowSignalLatitude))
                .ForMember(d => d.LowSignalLongitude, m => m.MapFrom(s => s.Positioning.LowSignalLongitude))
                .ForMember(d => d.HighSignalLevel, m => m.MapFrom(s => s.Positioning.HighSignalLevel))
                .ForMember(d => d.HighSignalLatitude, m => m.MapFrom(s => s.Positioning.HighSignalLatitude))
                .ForMember(d => d.HighSignalLongitude, m => m.MapFrom(s => s.Positioning.HighSignalLongitude))
                .ForMember(d => d.SignalRadius, m => m.MapFrom(s => s.Positioning.SignalRadius))
                .ForMember(d => d.SignalArea, m => m.MapFrom(s => s.Positioning.SignalArea))
                .ForMember(d => d.RawSecurityPayload, m => m.MapFrom(s => s.Security.RawSecurityPayload))
                .ForMember(d => d.SerializedSecurityPayload, m => m.MapFrom(s => s.Security.SerializedSecurityPayload))
                .ForMember(d => d.IsSecure, m => m.MapFrom(s => s.Security.IsSecure))
                .ForMember(d => d.Status, m => m.MapFrom(s => s.Status.Value));
        }
    }
}
