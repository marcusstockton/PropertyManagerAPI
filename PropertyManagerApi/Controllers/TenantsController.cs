using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PropertyManager.Api.Interfaces;
using PropertyManagerApi.Data;
using PropertyManagerApi.Models;
using PropertyManagerApi.Models.DTOs.Tenant;

namespace PropertyManagerApi.Controllers
{
    [Authorize]
    [Route("api/[controller]/{propertyId}")]
    [ApiController]
    public class TenantsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;

        public TenantsController(ApplicationDbContext context, IMapper mapper, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
            _mapper = mapper;
        }

        // GET: api/Tenants
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tenant>>> GetTenants()
        {
            return await _context.Tenants.ToListAsync();
        }

        // GET: api/Tenants/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tenant>> GetTenant(Guid id)
        {
            var tenant = await _context.Tenants.FindAsync(id);
            
            if (tenant == null)
            {
                return NotFound();
            }

            return tenant;
        }

        // PUT: api/Tenants/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTenant(Guid id, Tenant tenant)
        {
            if (id != tenant.Id)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                _context.Entry(tenant).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TenantExists(id))
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
            return BadRequest(ModelState);
        }

        // POST: api/Tenants
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Tenant>> PostTenant([FromForm]Tenant_CreateDto value)
        {
            if (ModelState.IsValid)
            {
                var newTenant = _mapper.Map<Tenant>(value);
                _context.Tenants.Add(newTenant);

                if (value.Profile != null)
                {
                    // Save the file, return the file location
                    newTenant.Profile_Url = await _fileService.SaveFile(value.Profile, newTenant.Id);
                }

                await _context.SaveChangesAsync();

                return Ok(newTenant);
            }
            return BadRequest(ModelState);
        }

        // DELETE: api/Tenants/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Tenant>> DeleteTenant(Guid id)
        {
            var tenant = await _context.Tenants.FindAsync(id);
            if (tenant == null)
            {
                return NotFound();
            }

            _context.Tenants.Remove(tenant);
            await _context.SaveChangesAsync();

            return tenant;
        }

        private bool TenantExists(Guid id)
        {
            return _context.Tenants.Any(e => e.Id == id);
        }
    }
}
