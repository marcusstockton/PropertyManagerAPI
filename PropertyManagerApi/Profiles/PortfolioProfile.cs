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
            CreateMap<Portfolio, PortfolioDetailDto>().ReverseMap();
            CreateMap<Portfolio, PortfolioListItemDto>()
                .ForMember(dest => dest.PropertyCount, opt => opt.MapFrom(src => src.Properties.Count))
                .ReverseMap();
        }
    }
}
