using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PropertyManagerApi.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string Token { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DoB { get; set; }
    }
}
