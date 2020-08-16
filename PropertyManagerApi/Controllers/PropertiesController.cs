using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropertyManagerApi.Interfaces;
using PropertyManagerApi.Models;
using PropertyManagerApi.Models.DTOs.Property;
using PropertyManagerApi.Models.DTOs.Tenant;

namespace PropertyManagerApi.Controllers
{
    //[Authorize]
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
        public async Task<ActionResult<List<PropertyDetail>>> GetPropertiesByPortfolioId(Guid portfolioId)
        {
            var propertyList = await _propertyService.GetPropertiesForPortfolio(portfolioId);

            var result = _mapper.Map<List<PropertyDetail>>(propertyList);
            return Ok(result);
        }

        //public async Task<ActionResult<IEnumerable<PropertyDetail>>> GetPropertiesByLoggedInUser()
        //{
        //    // Need to add in a owner property to Property for this to work
        //    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        //}

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

        [HttpGet("GetPropertyDetails/{id}")]
        public async Task<ActionResult<PropertyDetail>> GetPropertyDetails(Guid propertyId)
        {
            var @property = await _propertyService.GetPropertyDetails(propertyId);
            if (@property == null)
            {
                return NotFound();
            }
            var result = _mapper.Map<PropertyDetail>(@property);
            
            return result;
        }

        // PUT: api/Properties/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{portfolioId}/{id}")]
        public async Task<IActionResult> PutProperty(Guid portfolioId, Guid id, PropertyDetail propertyDetail)
        {
            if (id != propertyDetail.Id)
            {
                return BadRequest();
            }

            var property = _mapper.Map<Property>(propertyDetail);
            await _propertyService.UpdateProperty(property, portfolioId);

            return NoContent();
        }

        // POST: api/Properties
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("{portfolioId}")]
        public async Task<ActionResult<Property>> PostProperty(Guid portfolioId, PropertyCreate @property)
        {
            var newProperty = _mapper.Map<Property>(@property);
            await _propertyService.CreateProperty(newProperty, portfolioId);

            return Ok(newProperty);
        }

        // DELETE: api/Properties/5
        [HttpDelete("{portfolioId}/{id}")]
        public async Task<ActionResult<bool>> DeleteProperty(Guid portfolioId, Guid id)
        {
            return await _propertyService.DeleteProperty(id);
        }
    }
}
