using Microsoft.AspNetCore.Http;
using PropertyManagerApi.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PropertyManager.Api.Interfaces
{
    public interface ITenantService
    {
        Task<List<Tenant>> GetTenantsByPropertyId(Guid propertyId);

        Task<Tenant> GetTenantById(Guid tenantId);

        Task<bool> UpdateTenant(Guid tenantId, Tenant tenantModel);

        Task<Tenant> CreateTenant(Tenant tenantModel, IFormFile profile);

        Task<bool> DeleteTenant(Guid tenantId);
    }
}