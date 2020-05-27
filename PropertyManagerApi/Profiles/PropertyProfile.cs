using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PropertyManagerApi.Models.DTOs.Property;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
