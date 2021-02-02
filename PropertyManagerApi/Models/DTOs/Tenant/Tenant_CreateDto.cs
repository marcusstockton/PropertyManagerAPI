using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace PropertyManagerApi.Models.DTOs.Tenant
{
    public class Tenant_CreateDto
    {
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [DataType(DataType.PhoneNumber), Required]
        public string ContactNumber { get; set; }

        [DataType(DataType.EmailAddress), Required]
        public string EmailAddress { get; set; }

        [DataType(DataType.Date), Required]
        public DateTime TenancyStartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? TenancyEndDate { get; set; }

        public string Profession { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile Profile { get; set; }
    }
}