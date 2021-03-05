using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PropertyManagerApi.Data;
using PropertyManagerApi.Interfaces;
using PropertyManagerApi.Models;

namespace PropertyManagerApi.Services
{
    public class PortfolioService : IPortfolioServce
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PortfolioService> _logger;

        public PortfolioService(ApplicationDbContext context, ILogger<PortfolioService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Portfolio>> GetPortfolios(string userId)
        {
            return await _context.Portfolios
                .AsNoTracking()
                .Include(x => x.Owner)
                .Include(x => x.Properties)
                .Where(x => x.Owner.Id == Guid.Parse(userId))
                .ToListAsync();
        }

        public async Task<Portfolio> GetPortfolioById(Guid portfolioId)
        {
            return await _context.Portfolios
                .Include(x => x.Owner)
                .FirstOrDefaultAsync(x => x.Id == portfolioId);
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
            _logger.LogInformation("Trying to update portfolio id {PORTFOLIOID}", portfolio.Id);
            _context.Attach(portfolio);
            _context.Entry(portfolio).State = EntityState.Modified;
            try
            {
                _logger.LogInformation("Trying to save update to portfolio id {PORTFOLIOID}", portfolio.Id);
                await _context.SaveChangesAsync();
                return portfolio;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError("Error updating portfolio with id {PORTFOLIOID}. Error: {ERROR}", portfolio.Id, ex);
                return portfolio;
            }
            catch (ArgumentException ex)
            {
                _logger.LogError("Error updating portfolio with id {PORTFOLIOID}. Error: {ERROR}", portfolio.Id, ex);
                return portfolio;
            }
        }

        public async Task<Portfolio> CreatePortfolio(Portfolio portfolio, Guid userId)
        {
            _logger.LogInformation("Called into CreatePortfolio. Searching for user id {USERID}", userId);
            var loggedInUser = await _context.Users.FindAsync(userId);
            if (loggedInUser == null)
            {
                _logger.LogError("Unable to find a user with the userid {USERID}", userId);
            }
            _logger.LogInformation("Find user with userid {USERID}", userId);
            portfolio.Owner = loggedInUser;

            _logger.LogInformation("Trying to create the portfolio with name {PORTFOLIONAME} for user id {USERID}", portfolio.Name, userId);
            _context.Portfolios.Add(portfolio);
            try
            {
                _logger.LogInformation("Saving Portfolio {PORTFOLIONAME} for user id {USERID}", portfolio.Name, userId);
                await _context.SaveChangesAsync();
                return portfolio;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError("Unable to create a new portfolio {PORTFOLIONAME}. Error: {ERROR}", portfolio.Name, ex);
                throw;
            }
        }

        public async Task<bool> DeletePortfolio(Guid portfolioId)
        {
            _logger.LogInformation("Called into Delete Portfolio. Searching for Portfolio Id {PORTFOLIOID}", portfolioId);
            var portfolio = await _context.Portfolios.FindAsync(portfolioId);
            if (portfolio == null)
            {
                _logger.LogError("Unable to find a portfolio with Portfolio Id {PORTFOLIOID}", portfolioId);
                return false;
            }
            _logger.LogInformation("Portfolio {PORTFOLIONAME} found with Portfolio Id {PORTFOLIOID}. Attempting to Delete.", portfolio.Name, portfolioId);
            _context.Portfolios.Remove(portfolio);

            return (await _context.SaveChangesAsync() > 0);
        }
    }
}