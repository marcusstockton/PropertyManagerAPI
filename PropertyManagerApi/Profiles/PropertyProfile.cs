using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PropertyManagerApi.Models.DTOs.Property;

namespace PropertyManagerApi.Profiles
{
    public class PropertyProfile : Profile
    {
        public PropertyProfile()
        {
            CreateMap<Property, PropertyDetail>().ReverseMap();
        }
    }
}
