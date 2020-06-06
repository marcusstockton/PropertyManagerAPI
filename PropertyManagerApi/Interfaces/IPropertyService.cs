using PropertyManagerApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PropertyManagerApi.Interfaces
{
    public interface IPropertyService
    {
        Task<IList<Property>> GetPropertiesForPortfolio(Guid portfolioId);
        Task<Property> GetPropertyById(Guid portfolioId, Guid propertyId);
        Task<Property> CreateProperty(Property property);
        Task<Property> UpdateProperty(Property property);
        Task<bool> DeleteProperty(Guid propertyId);
    }
}
