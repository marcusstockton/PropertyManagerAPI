using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PropertyManagerApi.Data;
using PropertyManagerApi.Models;
using PropertyManagerApi.Services;

namespace PropertyManager.Api.Unittests.Services
{
    [TestClass()]
    public class PropertyServiceTests
    {
        private readonly DbContextOptions<ApplicationDbContext> _options;
        private readonly Mock<ILogger<PropertyService>> _logger = new Mock<ILogger<PropertyService>>();
        private readonly Mock<IUserService> _userServiceMock = new Mock<IUserService>();

        private Guid user1Id = Guid.NewGuid();
        private Guid user2Id = Guid.NewGuid();
        private Guid portfolio1Id = Guid.NewGuid();
        private Guid portfolio2Id = Guid.NewGuid();
        private Guid property1Id = Guid.NewGuid();

        public PropertyServiceTests()
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(databaseName: nameof(PropertyServiceTests) + Guid.NewGuid().ToString())
               .Options;

            using (var dbContext = new ApplicationDbContext(_options))
            {
                dbContext.Database.EnsureDeleted();
                dbContext.Database.EnsureCreated();
                SetupData(dbContext);
            }
        }

        [TestMethod()]
        public async Task GetPropertiesForPortfolio_Returns_PropertiesForCorrectPortfolio()
        {
            // Arrange
            using (var dbContext = new ApplicationDbContext(_options))
            {
                var propertyService = new PropertyService(dbContext, _userServiceMock.Object);

                // Act
                var result = await propertyService.GetPropertiesForPortfolio(portfolio1Id);
                var result2 = await propertyService.GetPropertiesForPortfolio(portfolio2Id);

                // Assert
                Assert.AreEqual(2, result.Count);
                Assert.AreEqual(0, result2.Count);
            }
        }

        [TestMethod()]
        public async Task GetPropertyById_Returns_CorrectProperty()
        {
            // Arrange
            using var dbContext = new ApplicationDbContext(_options);
            var propertyService = new PropertyService(dbContext, _userServiceMock.Object);

            // Act
            var result = await propertyService.GetPropertyById(portfolio1Id, property1Id);

            // Assert
            Assert.AreEqual("Description", result.Description);
            Assert.AreEqual(2, result.NoOfBeds);
            Assert.IsNull(result.Tenants);
        }

        [TestMethod()]
        public async Task GetPropertyTenantAndAddressDetails_Returns_CorrectPropertyDetails()
        {
            // Arrange
            using var dbContext = new ApplicationDbContext(_options);
            var propertyService = new PropertyService(dbContext, _userServiceMock.Object);

            // Act
            var result = await propertyService.GetPropertyTenantAndAddressDetails(property1Id);

            // Assert
            Assert.AreEqual("Description", result.Description);
            Assert.AreEqual(2, result.NoOfBeds);
            Assert.AreEqual(1, result.Tenants.Count);
        }

        [TestMethod()]
        public async Task CreateProperty_Returns_CorrectlyCreatesProperty()
        {
            // Arrange
            using var dbContext = new ApplicationDbContext(_options);
            var propertyService = new PropertyService(dbContext, _userServiceMock.Object);

            var newProp = new Property
            {
                Address = new Address
                {
                    City = "City",
                    Line1 = "Line 1",
                    PostCode = "Postcode"
                },
                Description = "Description for new property",
                NoOfBeds = 1,
                PropertyValue = 564332,
                PurchaseDate = DateTime.Now.AddDays(-567),
                PurchasePrice = 45325,
                RentalPrice = 543,
                Tenants = new List<Tenant>
                {
                    new Tenant
                    {
                        FirstName = "Ivy",
                        LastName = "Fatter",
                        Profession = "Sparky",
                        ContactNumber = "019283475",
                        EmailAddress = "test@Test.com",
                        TenancyStartDate = DateTime.Now.AddDays(-45),
                        Title = "Miss"
                    }
                }
            };

            // Act
            var result = await propertyService.CreateProperty(newProp, portfolio1Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.NoOfBeds);
            Assert.AreEqual("Description for new property", result.Description);
            Assert.AreEqual(1, result.Tenants.Count);
        }

        private void SetupData(ApplicationDbContext dbContext)
        {
            var portfolio1 = new Portfolio
            {
                Name = "Portfolio1",
                Id = portfolio1Id,
                Properties = new List<Property>
                {
                    new Property
                    {
                        Id = property1Id,
                        IsActive = true,
                        Address = new Address
                        {
                            IsActive = true,
                            City = "City",
                            Line1 = "Line 1",
                            PostCode = "EX11EX"
                        },
                        Description = "Description",
                        NoOfBeds = 2,
                        PropertyValue = 24680m,
                        PurchaseDate = DateTime.Now.AddDays(-409),
                        PurchasePrice = 12345,
                        RentalPrice = 458,
                        Tenants = new List<Tenant>
                        {
                            new Tenant
                            {
                                FirstName = "Dave",
                                LastName = "Matthews",
                                EmailAddress = "test@test.com",
                                ContactNumber = "04836749513",
                                Profession = "Belly Dancer",
                                Title = "Sir",
                                TenancyStartDate = DateTime.Now.AddDays(-89)
                            }
                        }
                    },
                    new Property
                    {
                        IsActive = true,
                        Address = new Address
                        {
                            IsActive = true,
                            City = "City",
                            Line1 = "Line 1",
                            PostCode = "EX11EX"
                        },
                        Description = "Description 2",
                        NoOfBeds = 1,
                        PropertyValue = 456732,
                        PurchaseDate = DateTime.Now.AddDays(-469),
                        PurchasePrice = 12345,
                        RentalPrice = 700
                    }
                }
            };

            var portfolio2 = new Portfolio
            {
                Name = "Portfolio2",
                Id = portfolio2Id
            };

            dbContext.Portfolios.AddRange(portfolio1, portfolio2);
            dbContext.SaveChanges();
        }
    }
}