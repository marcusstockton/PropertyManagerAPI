using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PropertyManager.Api.Interfaces;
using PropertyManagerApi.Data;
using PropertyManagerApi.Models;

namespace PropertyManager.Api.Services
{
    public class TenantService : ITenantService
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileService _fileService;

        public TenantService(ApplicationDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        public async Task<List<Tenant>> GetTenantsByPropertyId(Guid propertyId)
        {
            return await _context.Tenants.Where(x => x.Property.Id == propertyId).ToListAsync();
        }

        public async Task<Tenant> GetTenantById(Guid tenantId)
        {
            return await _context.Tenants.FindAsync(tenantId);
        }

        public async Task<bool> UpdateTenant(Guid tenantId, Tenant tenantModel)
        {
            var tenant = await _context.Tenants.FindAsync(tenantId);
            if (tenant != null)
            {
                _context.Entry(tenant).CurrentValues.SetValues(tenantModel);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<Tenant> CreateTenant(Tenant tenantModel, IFormFile profile)
        {
            await _context.Tenants.AddAsync(tenantModel);

            if (profile != null)
            {
                // Save the file, return the file location
                tenantModel.Profile_Url = await _fileService.SaveFile(profile, tenantModel.Id);
            }
            await _context.SaveChangesAsync();
            return tenantModel;
        }

        public async Task<bool> DeleteTenant(Guid tenantId)
        {
            var tenant = await _context.Tenants.FindAsync(tenantId);
            if (tenant != null)
            {
                _context.Tenants.Remove(tenant);
                return (await _context.SaveChangesAsync() > 0);
            }
            return false;
        }
    }
}