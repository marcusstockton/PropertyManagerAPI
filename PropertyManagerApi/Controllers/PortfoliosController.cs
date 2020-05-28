using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PropertyManagerApi.Data;
using PropertyManagerApi.Models;
using PropertyManagerApi.Models.DTOs.Portfolio;

namespace PropertyManagerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PortfoliosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public PortfoliosController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Portfolios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Portfolio>>> GetPortfolios()
        {
            return await _context.Portfolios.ToListAsync();
        }

        // GET: api/Portfolios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PortfolioDetail>> GetPortfolio(Guid id)
        {
            var portfolio = await _context.Portfolios.FindAsync(id);

            if (portfolio == null)
            {
                return NotFound();
            }
            var portfolioDetail = _mapper.Map<PortfolioDetail>(portfolio);
            return portfolioDetail;
        }

        [HttpGet("GetPortfolioAndProperties/{id}")]
        public async Task<ActionResult<PortfolioDetail>> GetPortfolioAndProperties(Guid id)
        {
            var portfolio = await _context.Portfolios
                .Include(x=>x.Properties)
                    .ThenInclude(x=>x.Address)
                .Include(x=>x.Properties)
                    .ThenInclude(x=>x.Tenants)
                .FirstOrDefaultAsync(x=>x.Id == id);

            if (portfolio == null)
            {
                return NotFound();
            }
            var portfolioDetail = _mapper.Map<PortfolioDetail>(portfolio);
            return portfolioDetail;
        }

        // PUT: api/Portfolios/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPortfolio(Guid id, PortfolioDetail portfolioDetail)
        {
            if (id != portfolioDetail.Id)
            {
                return BadRequest();
            }
            var portfolio = _mapper.Map<Portfolio>(portfolioDetail);

            _context.Entry(portfolio).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PortfolioExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Portfolios
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Portfolio>> PostPortfolio(PortfolioCreate portfolio)
        {
            var newPortfolio = _mapper.Map<Portfolio>(portfolio);
            var userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            Guid userId;
            if (string.IsNullOrEmpty(userid) || !Guid.TryParse(userid, out userId) )
            {
                return Unauthorized();
            }
            var loggedInUser = await _context.Users.FindAsync(userId);
            newPortfolio.Owner = loggedInUser;
            _context.Portfolios.Add(newPortfolio);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPortfolio", new { id = newPortfolio.Id }, newPortfolio);
        }

        // DELETE: api/Portfolios/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<PortfolioDetail>> DeletePortfolio(Guid id)
        {
            var portfolio = await _context.Portfolios.FindAsync(id);
            if (portfolio == null)
            {
                return NotFound();
            }

            var portfolioDetail = _mapper.Map<PortfolioDetail>(portfolio);

            _context.Portfolios.Remove(portfolio);

            await _context.SaveChangesAsync();

            return portfolioDetail;
        }

        private bool PortfolioExists(Guid id)
        {
            return _context.Portfolios.Any(e => e.Id == id);
        }
    }
}
