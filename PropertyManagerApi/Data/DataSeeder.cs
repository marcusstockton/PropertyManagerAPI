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
            if (!_context.Portfolios.Any())
            {
                // Create some portfolio's
                var p1 = new Portfolio
                {
                    Name = "Exeter 1",
                    Owner = _context.Users.First(x => x.Email == "beckystockton84@hotmail.co.uk"),
                    Properties = new System.Collections.Generic.List<Property>{
                        new Property{
                            Address = new Address{
                                City = "Exeter",
                                Line1 = "18B",
                                Line2 = "Whipton Close",
                                PostCode = "EX1 1EX",
                            },
                            Description = "A stunning one bedroom flat, decorated to a high standard for the area",
                            NoOfBeds = 1,
                            PropertyValue = 120987,
                            PurchaseDate = new DateTime(2001, 5, 12),
                            RentalPrice = 675,
                            Tenants = new System.Collections.Generic.List<Tenant>{
                                new Tenant{
                                    ContactNumber = "07836759231",
                                    EmailAddress = "testTenant@demo.com",
                                    FirstName = "Mallory",
                                    LastName = "Averland",
                                    Profession = "Bottle Doctor",
                                    TenancyStartDate = new DateTime(2019, 02, 16),
                                    Title = "Mr",
                                    Notes = new System.Collections.Generic.List<Note>{
                                        new Note{
                                            Title = "First Impressions",
                                            Description = "A lovely tenant, friendly and chatty, and looks like they will keep the property in good condition"
                                        },
                                    },
                                },
                                new Tenant{
                                    ContactNumber = "0784572834",
                                    EmailAddress = "old_tenant@test.com",
                                    FirstName = "Sioux",
                                    LastName = "Lamprey",
                                    Profession = "Cleaner",
                                    Title  = "Miss",
                                    TenancyStartDate = new DateTime(2018, 01, 12),
                                    TenancyEndDate = new DateTime(2019, 01, 30),
                                    Notes = new System.Collections.Generic.List<Note>{
                                        new Note{
                                            Title = "My first note",
                                            Description = "Looks shady, but promises to keep the property in 'good nick'"
                                        },

                                    },
                                }
                            }
                        }
                    }
                };
                await _context.Portfolios.AddAsync(p1);
                await _context.SaveChangesAsync();
            }
        }
    }
}
