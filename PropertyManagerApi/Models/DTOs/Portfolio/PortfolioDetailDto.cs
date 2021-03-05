using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PropertyManagerApi.Models.DTOs.Auth;
using PropertyManagerApi.Models.DTOs.Property;

namespace PropertyManagerApi.Models.DTOs.Portfolio
{
    public class PortfolioDetailDto
    {
        public Guid Id { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? UpdatedDateTime { get; set; }

        [MinLength(2), Required]
        public string Name { get; set; }

        public UserDto Owner { get; set; }
        public IList<PropertyDetailDto> Properties { get; set; }
    }
}