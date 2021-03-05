using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace PropertyManagerApi.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string Token { get; set; }

        [MaxLength(100)]
        public string FirstName { get; set; }

        [MaxLength(100)]
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DoB { get; set; }
    }
}