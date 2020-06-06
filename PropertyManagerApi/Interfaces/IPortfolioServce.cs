using PropertyManagerApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PropertyManagerApi.Interfaces
{
    public interface IPortfolioServce
    {
        Task<IEnumerable<Portfolio>> GetPortfolios(string userId);
        Task<Portfolio> GetPortfolioById(Guid portfolioId);
        Task<Portfolio> GetPortfolioAndProperties(Guid portfolioId);
        Task<Portfolio> UpdatePortfolio(Portfolio portfolio);
        Task<Portfolio> CreatePortfolio(Portfolio portfolio, Guid userId);
        Task<bool> DeletePortfolio(Guid portfolioId);
    }
}
