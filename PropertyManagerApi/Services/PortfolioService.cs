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
    public class PortfolioService : IPortfolioServce
    {
        private readonly ApplicationDbContext _context;
        public PortfolioService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Portfolio>> GetPortfolios(string userId)
        {
            return await _context.Portfolios.Where(x=>x.Owner.Id == Guid.Parse(userId)).ToListAsync();
        }

        public async Task<Portfolio> GetPortfolioById(Guid portfolioId)
        {
            return await _context.Portfolios.FindAsync(portfolioId);
        }
        public async Task<Portfolio> GetPortfolioAndProperties(Guid portfolioId)
        {
            return await _context.Portfolios
                .Include(x => x.Properties)
                    .ThenInclude(x => x.Address)
                .Include(x => x.Properties)
                    .ThenInclude(x => x.Tenants)
                .FirstOrDefaultAsync(x => x.Id == portfolioId);
        }

        public async Task<Portfolio> UpdatePortfolio(Portfolio portfolio)
        {
            _context.Attach(portfolio);
            _context.Entry(portfolio).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return portfolio;
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }

        public async Task<Portfolio> CreatePortfolio(Portfolio portfolio, Guid userId)
        {
            var loggedInUser = await _context.Users.FindAsync(userId);
            portfolio.Owner = loggedInUser;

            _context.Portfolios.Add(portfolio);
            try
            {
                await _context.SaveChangesAsync();
                return portfolio;
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }

        public async Task<bool> DeletePortfolio(Guid portfolioId)
        {
            var portfolio = await _context.Portfolios.FindAsync(portfolioId);

            _context.Portfolios.Remove(portfolio);

            return (await _context.SaveChangesAsync() > 0);
        }
    }
}
