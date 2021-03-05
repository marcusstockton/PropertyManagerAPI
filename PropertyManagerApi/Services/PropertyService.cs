using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PropertyManagerApi.Data;
using PropertyManagerApi.Interfaces;
using PropertyManagerApi.Models;

namespace PropertyManagerApi.Services
{
    public class PropertyService : IPropertyService
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserService _userService;

        public PropertyService(ApplicationDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public async Task<IList<Property>> GetPropertiesForPortfolio(Guid portfolioId)
        {
            return await _context.Properties
                .Include(x => x.Address)
                .Include(x => x.Tenants)
                .AsNoTracking()
                .Where(x => x.Portfolio.Id == portfolioId && x.IsActive)
                .Where(x => x.Portfolio.OwnerId == _userService.GetLoggedInUserId())
                .ToListAsync();
        }

        //public async Task<IList<Property>> GetPropertiesForUserId(Guid userId)
        //{
        //    return await _context.Properties.AsNoTracking().Where(x => x.Portfolio == portfolioId).ToListAsync();
        //}

        public async Task<Property> GetPropertyById(Guid portfolioId, Guid propertyId)
        {
            return await _context.Properties.SingleOrDefaultAsync(x => x.Id == propertyId && x.Portfolio.Id == portfolioId);
        }

        public async Task<Property> GetPropertyTenantAndAddressDetails(Guid propertyId)
        {
            return await _context.Properties
                .Include(x => x.Tenants)
                .Include(x => x.Address)
                .SingleAsync(x => x.Id == propertyId);
        }

        public async Task<Property> CreateProperty(Property property, Guid portfolioId)
        {
            var portfolio = await _context.Portfolios
                .Include(x => x.Properties)
                .SingleOrDefaultAsync(x => x.Id == portfolioId);

            if (portfolio == null)
            {
                throw new Exception("No Portfolio Found");
            }
            //property.Portfolio = portfolio;

            portfolio.Properties.Add(property);
            await _context.SaveChangesAsync();

            return property;
        }

        public async Task<Property> AddAddressToProperty(Guid propertyId, Address address)
        {
            var property = await _context.Properties.FindAsync(propertyId);
            if (property != null)
            {
                property.Address = address;
                _context.Properties.Update(property);
                await _context.SaveChangesAsync();
                return property;
            }
            return null;
        }

        public async Task<Property> AddTenantsToProperty(Guid propertyId, List<Tenant> tenants)
        {
            var property = await _context.Properties.FindAsync(propertyId);
            if (property != null)
            {
                property.Tenants.AddRange(tenants);
                _context.Properties.Update(property);
                await _context.SaveChangesAsync();
                return property;
            }
            return null;
        }

        public async Task<Property> UpdateProperty(Property property, Guid portfolioId)
        {
            _context.Properties.Update(property);
            await _context.SaveChangesAsync();
            return property;
        }

        public async Task<bool> DeleteProperty(Guid portfolioId, Guid propertyId)
        {
            var property = await _context.Properties.FindAsync(propertyId);
            if (property != null)
            {
                if (property.PortfolioId == portfolioId)
                {
                    _context.Properties.Remove(property);
                    return (await _context.SaveChangesAsync() > 0);
                }
            }
            return false;
        }
    }
}