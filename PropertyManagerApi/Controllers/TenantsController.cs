using System;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropertyManager.Api.Interfaces;
using PropertyManagerApi.Models;
using PropertyManagerApi.Models.DTOs.Tenant;

namespace PropertyManagerApi.Controllers
{
    [Authorize]
    [Route("api/[controller]/{propertyId}")]
    [ApiController]
    public class TenantsController : ControllerBase
    {
        private readonly ITenantService _tenantService;
        private readonly IMapper _mapper;

        public TenantsController(IMapper mapper, ITenantService tenantService)
        {
            _mapper = mapper;
            _tenantService = tenantService;
        }

        // GET: api/Tenants/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tenant_DetailDto>> GetTenantById(Guid id)
        {
            var tenant = await _tenantService.GetTenantById(id);

            if (tenant == null)
            {
                return NotFound();
            }

            return _mapper.Map<Tenant_DetailDto>(tenant);
        }

        // PUT: api/Tenants/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTenant(Guid id, Tenant_DetailDto tenantDto)
        {
            if (id != tenantDto.Id)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                var tenant = _mapper.Map<Tenant>(tenantDto);

                await _tenantService.UpdateTenant(id, tenant);

                return NoContent();
            }
            return BadRequest(ModelState);
        }

        // POST: api/Tenants
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Tenant_DetailDto>> CreateTenant([FromForm] Tenant_CreateDto value)
        {
            if (ModelState.IsValid)
            {
                var newTenant = _mapper.Map<Tenant>(value);
                var createdTenant = await _tenantService.CreateTenant(newTenant, value.Profile);

                return Ok(createdTenant);
            }
            return BadRequest(ModelState);
        }

        // DELETE: api/Tenants/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteTenant(Guid id)
        {
            return await _tenantService.DeleteTenant(id);
        }

        [HttpGet("job-title-autocomplete")]
        public async Task<ActionResult> JobTitleAutoComplete(string jobTitle)
        {
            var url = $"http://api.dataatwork.org/v1/jobs/autocomplete?begins_with={jobTitle}";
            HttpClient req = new HttpClient();
            var content = await req.GetAsync(url);
            if (content.IsSuccessStatusCode)
            {
                return Ok(content.Content.ReadAsStringAsync());
            }
            return BadRequest(content);
        }
    }
}