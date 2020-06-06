using Microsoft.EntityFrameworkCore;
using PropertyManagerApi.Data;
using PropertyManagerApi.Interfaces;
using PropertyManagerApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PropertyManagerApi.Services
{
    public class PropertyService : IPropertyService
    {
        private readonly ApplicationDbContext _context;
        public PropertyService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IList<Property>> GetPropertiesForPortfolio(Guid portfolioId)
        {
            return await _context.Properties.AsNoTracking().Where(x => x.Portfolio.Id == portfolioId).ToListAsync();
        }

        public async Task<Property> GetPropertyById(Guid portfolioId, Guid propertyId)
        {
            return await _context.Properties.SingleOrDefaultAsync(x=>x.Id == propertyId && x.Portfolio.Id == portfolioId);
        }

        public async Task<Property> CreateProperty(Property property)
        {
            await _context.Properties.AddAsync(property);
            await _context.Addresses.AddAsync(property.Address);
            await _context.SaveChangesAsync();
            
            return property;
        }

        public async Task<Property> UpdateProperty(Property property)
        {
            _context.Properties.Update(property);
            await _context.SaveChangesAsync();
            return property;
        }

        public async Task<bool> DeleteProperty(Guid propertyId)
        {
            var property = await _context.Properties.FindAsync(propertyId);
            if (property != null)
            {
                _context.Properties.Remove(property);
                return (await _context.SaveChangesAsync() > 0);
            }
            return false;
        }
    }
}
