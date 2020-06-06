using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropertyManagerApi.Interfaces;
using PropertyManagerApi.Models;
using PropertyManagerApi.Models.DTOs.Property;

namespace PropertyManagerApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PropertiesController : ControllerBase
    {
        private readonly IPropertyService _propertyService;
        private readonly IMapper _mapper;

        public PropertiesController(IPropertyService propertyService, IMapper mapper)
        {
            _propertyService = propertyService;
            _mapper = mapper;
        }

        // GET: api/Properties
        /// <summary>
        /// Gets a list of properties against the given portfolio Id
        /// </summary>
        /// <param name="portfolioId">The Portfolio Id</param>
        /// <returns></returns>
        [HttpGet("{portfolioId}")]
        public async Task<ActionResult<IEnumerable<PropertyDetail>>> GetProperties(Guid portfolioId)
        {
            var propertyList = await _propertyService.GetPropertiesForPortfolio(portfolioId);

            var result = _mapper.Map<IEnumerable<PropertyDetail>>(propertyList);
            return Ok(result);
        }

        // GET: api/Properties/5
        [HttpGet("{portfolioId}/{id}")]
        public async Task<ActionResult<Property>> GetProperty(Guid portfolioId, Guid id)
        {
            var @property = await _propertyService.GetPropertyById(portfolioId, id);

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
        public async Task<IActionResult> PutProperty(Guid id, PropertyDetail propertyDetail)
        {
            if (id != propertyDetail.Id)
            {
                return BadRequest();
            }

            var property = _mapper.Map<Property>(propertyDetail);
            await _propertyService.UpdateProperty(property);

            return NoContent();
        }

        // POST: api/Properties
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("{portfolioId}")]
        public async Task<ActionResult<Property>> PostProperty(Guid portfolioId, PropertyCreate @property)
        {
            var newProperty = _mapper.Map<Property>(@property);
            newProperty.Portfolio.Id = portfolioId;
            await _propertyService.CreateProperty(newProperty);

            return CreatedAtAction(nameof(GetProperty), new { portfolioId, id = newProperty.Id });
        }

        // DELETE: api/Properties/5
        [HttpDelete("{portfolioId}/{id}")]
        public async Task<ActionResult<bool>> DeleteProperty(Guid portfolioId, Guid id)
        {
            return await _propertyService.DeleteProperty(id);
        }
    }
}
