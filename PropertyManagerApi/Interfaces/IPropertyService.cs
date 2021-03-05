using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PropertyManagerApi.Models;

namespace PropertyManagerApi.Interfaces
{
    public interface IPropertyService
    {
        Task<IList<Property>> GetPropertiesForPortfolio(Guid portfolioId);

        Task<Property> GetPropertyById(Guid portfolioId, Guid propertyId);

        Task<Property> CreateProperty(Property property, Guid portfolioId);

        Task<Property> UpdateProperty(Property property, Guid portfolioId);

        Task<bool> DeleteProperty(Guid portfolioId, Guid propertyId);

        Task<Property> GetPropertyTenantAndAddressDetails(Guid propertyId);

        Task<Property> AddAddressToProperty(Guid propertyId, Address address);

        Task<Property> AddTenantsToProperty(Guid propertyId, List<Tenant> tenants);
    }
}