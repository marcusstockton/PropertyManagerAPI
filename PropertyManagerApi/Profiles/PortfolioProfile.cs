using AutoMapper;
using PropertyManager.Api.Models.DTOs.Portfolio;
using PropertyManagerApi.Models;
using PropertyManagerApi.Models.DTOs.Portfolio;

namespace PropertyManagerApi.Profiles
{
    public class PortfolioProfile : Profile
    {
        public PortfolioProfile()
        {
            CreateMap<Portfolio, PortfolioCreateDto>().ReverseMap();
            CreateMap<Portfolio, PortfolioDetailDto>()
                .ForMember(x=>x.Owner, dest=>dest.MapFrom(x=>x.Owner)) // Causing me issues.
                .ReverseMap();
            CreateMap<Portfolio, PortfolioListItemDto>()
                .ForMember(dest => dest.PropertyCount, opt => opt.MapFrom(src => src.Properties.Count))
                .ReverseMap();
        }
    }
}
