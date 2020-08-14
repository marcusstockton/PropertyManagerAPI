﻿using PropertyManagerApi.Models;
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
        Task<Property> CreateProperty(Property property, Guid portfolioId);
        Task<Property> UpdateProperty(Property property, Guid portfolioId);
        Task<bool> DeleteProperty(Guid propertyId);
        Task<Property> GetPropertyDetails(Guid propertyId);
        Task<Property> AddAddressToProperty(Guid propertyId, Address address);
        Task<Property> AddTenantsToProperty(Guid propertyId, List<Tenant> tenants);
    }
}