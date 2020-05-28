using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PropertyManagerApi.Data;
using PropertyManagerApi.Models;
using PropertyManagerApi.Models.DTOs.Property;

namespace PropertyManagerApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PropertiesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public PropertiesController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Properties
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Property>>> GetProperties()
        //{
        //    return await _context.Properties.ToListAsync();
        //}

        // GET: api/Properties/5
        [HttpGet("{portfolioId}/{id}")]
        public async Task<ActionResult<Property>> GetProperty(Guid portfolioId, Guid id)
        {
            var @property = await _context.Properties.FindAsync(id);

            if (@property == null)
            {
                return NotFound();
            }

            return @property;
        }

        // PUT: api/Properties/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{portfolioId}/{id}")]
        public async Task<IActionResult> PutProperty(Guid portfolioId, Guid id, Property @property)
        {
            if (id != @property.Id)
            {
                return BadRequest();
            }

            _context.Entry(@property).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PropertyExists(id))
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

        // POST: api/Properties
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("{portfolioId}")]
        public async Task<ActionResult<Property>> PostProperty(Guid portfolioId, PropertyCreate @property)
        {
            var portfolio = await _context.Portfolios.FindAsync(portfolioId);
            if (portfolio == null)
            {
                return BadRequest("Portfolio not found");
            }
            var newProperty = _mapper.Map<Property>(@property);
            newProperty.Portfolio = portfolio;

            _context.Properties.Add(newProperty);
            _context.Addresses.Add(newProperty.Address);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProperty", new { id = newProperty.Id });
        }

        // DELETE: api/Properties/5
        [HttpDelete("{portfolioId}/{id}")]
        public async Task<ActionResult<Property>> DeleteProperty(Guid portfolioId, Guid id)
        {
            var @property = await _context.Properties.FindAsync(id);
            if (@property == null)
            {
                return NotFound();
            }

            _context.Properties.Remove(@property);
            await _context.SaveChangesAsync();

            return @property;
        }

        private bool PropertyExists(Guid id)
        {
            return _context.Properties.Any(e => e.Id == id);
        }
    }
}
