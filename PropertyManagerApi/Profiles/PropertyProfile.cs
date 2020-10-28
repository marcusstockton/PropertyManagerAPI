using AutoMapper;
using PropertyManagerApi.Models;
using PropertyManagerApi.Models.DTOs.Property;
using PropertyManagerApi.Models.DTOs.Tenant;
using System.Collections.Generic;
using System.Linq;

namespace PropertyManagerApi.Profiles
{
    public class PropertyProfile : Profile
    {
        public PropertyProfile()
        {
            CreateMap<Property, PropertyDetailDto>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.NoOfBeds, opt => opt.MapFrom(src => src.NoOfBeds))
                .ForMember(dest => dest.PropertyValue, opt => opt.MapFrom(src => src.PropertyValue))
                .ForMember(dest => dest.PurchaseDate, opt => opt.MapFrom(src => src.PurchaseDate))
                .ForMember(dest => dest.PurchasePrice, opt => opt.MapFrom(src => src.PurchasePrice))
                .ForMember(dest => dest.RentalPrice, opt => opt.MapFrom(src => src.RentalPrice))
                .ForMember(dest => dest.Tenants, opt => opt.MapFrom(src=>src.Tenants))
                .ReverseMap();

            CreateMap<Property, PropertyCreateDto>()
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
