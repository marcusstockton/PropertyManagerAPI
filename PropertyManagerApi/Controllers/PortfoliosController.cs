using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PropertyManagerApi.Data;
using PropertyManagerApi.Interfaces;
using PropertyManagerApi.Models;
using PropertyManagerApi.Models.DTOs.Portfolio;

namespace PropertyManagerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PortfoliosController : ControllerBase
    {
        private readonly IPortfolioServce _portfolioService;
        private readonly IMapper _mapper;

        public PortfoliosController(IPortfolioServce portfolioServce, IMapper mapper)
        {
            _portfolioService = portfolioServce;
            _mapper = mapper;
        }

        // GET: api/Portfolios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Portfolio>>> GetPortfolios()
        {
            var userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

            return Ok(await _portfolioService.GetPortfolios(userid));
        }

        // GET: api/Portfolios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PortfolioDetail>> GetPortfolio(Guid id)
        {
            var portfolio = await _portfolioService.GetPortfolioById(id);

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
            var portfolio = await _portfolioService.GetPortfolioAndProperties(id);

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

            await _portfolioService.UpdatePortfolio(portfolio);

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

            var createdPortfolio = await _portfolioService.CreatePortfolio(newPortfolio, userId);

            return CreatedAtAction(nameof(GetPortfolio), new { id = createdPortfolio.Id }, createdPortfolio);
        }

        // DELETE: api/Portfolios/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePortfolio(Guid id)
        {
            await _portfolioService.DeletePortfolio(id);
            
            return NoContent();
        }
    }
}
