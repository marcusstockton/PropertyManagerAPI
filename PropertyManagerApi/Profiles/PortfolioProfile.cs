using AutoMapper;
using PropertyManagerApi.Models;
using PropertyManagerApi.Models.DTOs.Portfolio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
