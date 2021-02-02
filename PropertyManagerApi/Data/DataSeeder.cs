using AutoBogus;
using Bogus.Extensions;
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
                    LastName = "Testman",
                    DoB = new DateTime(1986, 12, 15)
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
                    LastName = "Testman",
                    DoB = new DateTime(1984, 05, 09)
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
                var jobs = new[] { "Cleaner", "Software Developer", "Journal Manager", "Head of Marketing", "Mechanic", "Shop Assistant", "Builder", "Electrician", "Accountant",
                    "Nurse Practitioner", "Librarian", "Dog Trainer", "Delivery Driver", "Yoga Instructor", "Tech Support", "Cyber Security", "Biomedical Engineering", "Iron and Steel Worker",
                    "Sheet Metal Worker", "Clinical Nurse Specialist", "Nurse Practitioner", "Account Management","Quality Executive", "Risk Executive", "Accounting Specialist", "Accountant"};
                var maleTitles = new[] { "Dr", "Mr", "Rev", "Sr", "Prof", "Lord" };
                var femaleTitles = new[] { "Mrs", "Miss", "Ms", "Lady", "Mx" };
                var townNames = new[] { "Ossett", "Bradley Stoke", "Malton", "Swaffham", "Ramsey", "Saffron Walden", "Stockton-on-Tees", "Knares", "Horley",
                    "Ampthill", "Boston", "Sleaford", "Wakefield", "Hingham", "Northwich", "Canterbury", "Wadebridge", "Fowey", "Reading", "Wymondham", "Warrington",
                    "Northleach with Eastington", "Brentford", "Portsmouth", "Crediton", "Exeter", "Paignton", "Chester-le-Street", };
                var users = _context.Users.ToArray();
                var titles = maleTitles.Concat(femaleTitles);

                Random rnd = new Random();

                AutoFaker.Configure(builder =>
                {
                    builder.WithLocale("en_GB");
                });

                var notes = new AutoFaker<Note>()
                    .RuleFor(x => x.Description, f => f.Lorem.Paragraph(rnd.Next(3, 5)))
                    .RuleFor(x => x.Title, f => f.Lorem.Sentence(rnd.Next(2, 5)));

                var personFaker = new AutoFaker<Tenant>()
                      .RuleFor(x => x.Id, f => f.Random.Guid())
                      .RuleFor(x => x.FirstName, f => f.Person.FirstName)
                      .RuleFor(x => x.LastName, f => f.Person.LastName)
                      .RuleFor(x => x.Profession, f => f.PickRandom(jobs))
                      .RuleFor(x => x.ContactNumber, f => f.Phone.PhoneNumber())
                      .RuleFor(x => x.EmailAddress, f => f.Person.Email)
                      .RuleFor(x => x.Profile_Url, f => f.Person.Avatar)
                      .RuleFor(x => x.TenancyStartDate, f => f.Date.PastOffset(1).DateTime)
                      .RuleFor(x => x.TenancyEndDate, f => f.Date.PastOffset(1).DateTime.OrNull(f))
                      .RuleFor(x => x.Title, f => f.PickRandom(titles))
                      .RuleFor(x => x.CreatedDateTime, f => f.Date.Past())
                      .RuleFor(x => x.Notes, notes.Generate(rnd.Next(1, 4)));

                var addresses = new AutoFaker<Address>()
                    .RuleFor(x => x.City, f => f.Address.City())
                    .RuleFor(x => x.Line1, f => f.Address.StreetAddress())
                    .RuleFor(x => x.Line2, f => f.Address.SecondaryAddress())
                    .RuleFor(x => x.PostCode, f => f.Address.ZipCode("??# #??"))
                    .RuleFor(x => x.Town, f => f.PickRandom(townNames));

                var properties = new AutoFaker<Property>()
                    .RuleFor(x => x.AddressId, f => addresses.Generate().Id)
                    .RuleFor(x => x.Description, f => f.Lorem.Paragraph(rnd.Next(3, 7)))
                    .RuleFor(x => x.NoOfBeds, f => f.Random.Int(1, 4))
                    .RuleFor(x => x.PropertyValue, f => Math.Round(f.Random.Decimal(50000, 130000), 2))
                    .RuleFor(x => x.PurchasePrice, f => Math.Round(f.Random.Decimal(42000, 120000), 2))
                    .RuleFor(x => x.RentalPrice, f => Math.Round(f.Random.Decimal(400, 1300), 2))
                    .RuleFor(x => x.Tenants, personFaker.Generate(rnd.Next(2, 6)))
                    .RuleFor(x => x.PurchaseDate, f => f.Date.Past().Date);

                var portfolioList = new AutoFaker<Portfolio>()
                    .RuleFor(x => x.Properties, properties.Generate(rnd.Next(2, 5)))
                    .RuleFor(x => x.OwnerId, f => f.PickRandom(users).Id)
                    .RuleFor(x => x.Name, f => f.PickRandom(townNames));

                var portList = portfolioList.Generate(10);

                await _context.Portfolios.AddRangeAsync(portList);
                await _context.SaveChangesAsync();
            }
        }
    }
}