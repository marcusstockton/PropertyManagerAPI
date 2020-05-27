using AutoMapper;
using PropertyManagerApi.Models;
using PropertyManagerApi.Models.DTOs.Portfolio;

namespace PropertyManagerApi.Profiles
{
    public class PortfolioProfile :Profile
    {
        public PortfolioProfile()
        {
            CreateMap<Portfolio, PortfolioCreate>().ReverseMap();
            CreateMap<Portfolio, PortfolioDetail>().ReverseMap();
        }
    }
}
