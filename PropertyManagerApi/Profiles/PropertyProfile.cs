using AutoMapper;
using PropertyManagerApi.Models;
using PropertyManagerApi.Models.DTOs.Property;

namespace PropertyManagerApi.Profiles
{
    public class PropertyProfile : Profile
    {
        public PropertyProfile()
        {
            CreateMap<Property, PropertyDetail>().ReverseMap();

            CreateMap<Property, PropertyCreate>()
                .ForMember(dest => dest.AddressLine1, opt => opt.MapFrom(src => src.Address.Line1))
                .ForMember(dest => dest.AddressLine2, opt => opt.MapFrom(src => src.Address.Line2))
                .ForMember(dest => dest.AddressLine3, opt => opt.MapFrom(src => src.Address.Line3))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
                .ForMember(dest => dest.Town, opt => opt.MapFrom(src => src.Address.Town))
                .ForMember(dest => dest.Postcode, opt => opt.MapFrom(src => src.Address.PostCode))
                .ReverseMap();
        }
    }
}
