using PropertyManagerApi.Models.DTOs.Auth;
using PropertyManagerApi.Models.DTOs.Property;
using System;
using System.Collections.Generic;

namespace PropertyManagerApi.Models.DTOs.Portfolio
{
    public class PortfolioDetail
    {
        public Guid Id { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public string Name { get; set; }
        public UserDto Owner { get; set; }
        public IList<PropertyDetail> Properties { get; set; }
    }
}
