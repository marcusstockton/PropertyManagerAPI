using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PropertyManagerApi.Models.DTOs.Auth
{
    public class UserResponse
    {
        public string Token { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
