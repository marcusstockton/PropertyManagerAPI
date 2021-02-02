using AutoMapper;
using PropertyManager.Api.Models.DTOs.Address;
using PropertyManagerApi.Models;

namespace PropertyManager.Api.Profiles
{
    public class AddressProfile : Profile
    {
        public AddressProfile()
        {
            CreateMap<Address, AddressDetailsDto>().ReverseMap();
        }
    }
}