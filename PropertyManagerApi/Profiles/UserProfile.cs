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
            CreateMap<ApplicationUser, UserDto>().ReverseMap();
        }
    }
}
