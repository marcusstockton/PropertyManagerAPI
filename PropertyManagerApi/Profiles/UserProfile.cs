using AutoMapper;
using PropertyManagerApi.Models;
using PropertyManagerApi.Models.DTOs.Auth;

namespace PropertyManagerApi.Profiles
{
    /// <summary>
    /// User Profile for automapping
    /// </summary>
    public class UserProfile : Profile
    {
        /// <summary>
        /// Constructor for UserProfile
        /// </summary>
        public UserProfile()
        {
            CreateMap<ApplicationUser, UserResponse>().ReverseMap();
            CreateMap<ApplicationUser, UserDto>()
                .ForMember(x => x.DateOfBirth, opt => opt.MapFrom(opt => opt.DoB))
                .ForMember(x => x.FirstName, opt => opt.MapFrom(opt => opt.FirstName))
                .ForMember(x => x.LastName, opt => opt.MapFrom(opt => opt.LastName))
                .ForMember(x => x.FullName, opt => opt.MapFrom(opt => opt.FirstName + " " + opt.LastName))
                .ForMember(x => x.Username, opt => opt.MapFrom(opt => opt.UserName))
                .ReverseMap();
        }
    }
}
