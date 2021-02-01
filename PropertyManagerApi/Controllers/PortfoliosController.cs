using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropertyManager.Api.Models.DTOs.Portfolio;
using PropertyManagerApi.Interfaces;
using PropertyManagerApi.Models;
using PropertyManagerApi.Models.DTOs.Portfolio;

namespace PropertyManagerApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
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
        public async Task<ActionResult<IEnumerable<PortfolioListItemDto>>> GetPortfolios()
        {
            var userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var results = await _portfolioService.GetPortfolios(userid);

            return Ok(_mapper.Map<IEnumerable<PortfolioListItemDto>>(results));
        }

        // GET: api/Portfolios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PortfolioDetailDto>> GetPortfolioById(Guid id)
        {
            var portfolio = await _portfolioService.GetPortfolioById(id);

            if (portfolio == null)
            {
                return NotFound();
            }
            var portfolioDetail = _mapper.Map<PortfolioDetailDto>(portfolio);
            return portfolioDetail;
        }

        [HttpGet("GetPortfolioAndProperties/{id}")]
        public async Task<ActionResult<PortfolioDetailDto>> GetPortfolioAndProperties(Guid id)
        {
            var portfolio = await _portfolioService.GetPortfolioAndProperties(id);

            if (portfolio == null)
            {
                return NotFound();
            }
            var portfolioDetail = _mapper.Map<PortfolioDetailDto>(portfolio);
            return portfolioDetail;
        }

        // PUT: api/Portfolios/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePortfolio(Guid id, PortfolioDetailDto portfolioDetail)
        {
            if (id != portfolioDetail.Id)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                var portfolio = _mapper.Map<Portfolio>(portfolioDetail);

                await _portfolioService.UpdatePortfolio(portfolio);

                return NoContent();
            }
            return BadRequest(ModelState);
        }

        // POST: api/Portfolios
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Portfolio>> CreatePortfolio(PortfolioCreateDto portfolio)
        {
            if (ModelState.IsValid)
            {
                var newPortfolio = _mapper.Map<Portfolio>(portfolio);
                var userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
                Guid userId;
                if (string.IsNullOrEmpty(userid) || !Guid.TryParse(userid, out userId))
                {
                    return Unauthorized();
                }

                var createdPortfolio = await _portfolioService.CreatePortfolio(newPortfolio, userId);

                return CreatedAtAction(nameof(GetPortfolioById), new { id = createdPortfolio.Id }, createdPortfolio);
            }
            return BadRequest(ModelState);
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
