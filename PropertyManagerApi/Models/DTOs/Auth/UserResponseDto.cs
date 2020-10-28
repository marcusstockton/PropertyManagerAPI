using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PropertyManagerApi.Models.DTOs.Auth
{
    public class UserResponseDto
    {
        public string Token { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string EmailAddress { get; set; }
    }
}
