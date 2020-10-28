using PropertyManagerApi.Models.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PropertyManager.Api.Models.DTOs.Portfolio
{
    public class PortfolioListItemDto
    {
        public Guid Id { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? UpdatedDateTime { get; set; }

        public string Name { get; set; }

        public UserDto Owner { get; set; }

        public int PropertyCount { get; set; }
    }
}
