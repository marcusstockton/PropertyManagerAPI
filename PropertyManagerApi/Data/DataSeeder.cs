using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using PropertyManagerApi.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PropertyManagerApi.Data
{
    public class DataSeeder
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public DataSeeder(ApplicationDbContext context, IServiceProvider service, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<Guid>> roleManager)
        {
            _context = service.GetRequiredService<ApplicationDbContext>();
            _roleManager = service.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
            _userManager = userManager;
        }
        public async Task SeedData()
        {
            if (!_context.Roles.Any())
            {
                var role = new IdentityRole<Guid>();
                role.Name = "Admin";
                await _roleManager.CreateAsync(role);

                var basicRole = new IdentityRole<Guid>();
                basicRole.Name = "Basic";
                await _roleManager.CreateAsync(basicRole);

                await _context.SaveChangesAsync();
            }

            if (!_context.Users.Any())
            {
                var validUser = new ApplicationUser()
                {
                    Email = "test@test.com",
                    UserName = "testUser",
                    FirstName = "Marcus",
                    LastName = "Stockton",
                    DoB = new DateTime(1985, 4, 12)
                };
                var validUserResult = await _userManager.CreateAsync(validUser, "Pa$$w0rd");
                if (validUserResult.Succeeded)
                {
                    await _userManager.AddToRoleAsync(validUser, "Admin");
                }
                
                var basicUser = new ApplicationUser()
                {
                    Email = "beckystockton84@hotmail.co.uk",
                    UserName = "beckystockton84@hotmail.co.uk",
                    FirstName = "Becky",
                    LastName = "Stockton",
                    DoB = new DateTime(1993, 1, 16)
                };

                var basicUserResult = await _userManager.CreateAsync(basicUser, "Pa$$w0rd");
                if (basicUserResult.Succeeded)
                {
                    await _userManager.AddToRoleAsync(basicUser, "Basic");
                }

                _context.SaveChanges();
            }

        }
        }
}
