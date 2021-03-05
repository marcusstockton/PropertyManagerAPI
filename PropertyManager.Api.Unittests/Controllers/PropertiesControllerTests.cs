using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PropertyManager.Api.Profiles;
using PropertyManagerApi.Interfaces;
using PropertyManagerApi.Models;
using PropertyManagerApi.Models.DTOs.Property;
using PropertyManagerApi.Profiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace PropertyManagerApi.Controllers.Tests
{
    [TestClass()]
    public class PropertiesControllerTests
    {
        private Mock<IPropertyService> _propertyServiceMock;
        private IMapper _mapperMock;
        private ApplicationUser _user;
        private ApplicationUser _user2;
        private GenericIdentity _identity;
        private ClaimsPrincipal _principle;

        [TestInitialize]
        public void Setup()
        {
            // Runs before each test. (Optional)

            // Mocks a logged in user:
            _user = new ApplicationUser() { UserName = "JohnDoe", Id = Guid.NewGuid() };
            _user2 = new ApplicationUser() { UserName = "JaneDoe", Id = Guid.NewGuid() };
            _identity = new GenericIdentity(_user.Id.ToString());
            _principle = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, _user.UserName),
                new Claim(ClaimTypes.NameIdentifier, _user.Id.ToString()),
                new Claim("custom-claim", "example claim value"),
            }, "mock"));

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new PropertyProfile());
                mc.AddProfile(new UserProfile());
                mc.AddProfile(new AddressProfile());
                mc.AddProfile(new TenantProfile());
            });

            _mapperMock = new Mapper(mappingConfig);

            _propertyServiceMock = new Mock<IPropertyService>();
        }

        [TestMethod()]
        public async Task GetPropertiesByPortfolioIdAsyncReturnsCorrectProperties()
        {
            var portfolio1 = new Portfolio { Name = "Portfolio1" };
            var portfolio2 = new Portfolio { Name = "Portfolio2" };

            var properties = new List<Property> {
                new Property
                {
                    Address = new Address{
                        City = "Exeter",
                        CreatedDateTime = DateTime.Now.AddDays(-3),
                        IsActive = true,
                        Line1 = "Line 1",
                        Line2 = "Line 2",
                        PostCode = "Postcode",
                        Town = "Town"
                    },
                    Description = "Description",
                    IsActive= true,
                    NoOfBeds = 2,
                    PropertyValue = 12345,
                    PurchaseDate = new DateTime(2008, 3, 1),
                    PurchasePrice = 12343,
                    RentalPrice = 431,
                    CreatedDateTime = DateTime.Now.AddDays(-1),
                    Portfolio = portfolio1,
                    Tenants = new List<Tenant>{
                        new Tenant{ FirstName = "FirstName1", LastName = "LastName1", EmailAddress = "test1@test.com", IsActive = true, Profession = "Window cleaner", TenancyStartDate = DateTime.Now.AddDays(-1) },
                        new Tenant{ FirstName = "FirstName2", LastName = "LastName2", EmailAddress = "test2@test.com", IsActive = true, Profession = "Superstar DJ", TenancyStartDate = DateTime.Now.AddDays(-1) }
                    }
                }, new Property
                {
                    Address = new Address{
                        City = "Plymouth",
                        CreatedDateTime = DateTime.Now.AddDays(-3),
                        IsActive = true,
                        Line1 = "Line 1",
                        Line2 = "Line 2",
                        PostCode = "Postcode",
                        Town = "Town"
                    },
                    Description = "Description Two",
                    IsActive= true,
                    NoOfBeds = 2,
                    PropertyValue = 76547,
                    PurchaseDate = new DateTime(2008, 3, 1),
                    PurchasePrice = 75273,
                    RentalPrice = 654,
                    CreatedDateTime = DateTime.Now.AddDays(-1),
                    Portfolio = portfolio1,
                    Tenants = new List<Tenant>{
                        new Tenant{ FirstName = "FirstName3", LastName = "LastName3", EmailAddress = "test3@test.com", IsActive = true, Profession = "Window cleaner", TenancyStartDate = DateTime.Now.AddDays(-1) },
                    }
                },
                new Property
                {
                    Address = new Address{
                        City = "Saltash",
                        CreatedDateTime = DateTime.Now.AddDays(-3),
                        IsActive = true,
                        Line1 = "Line 1",
                        Line2 = "Line 2",
                        PostCode = "Postcode",
                        Town = "Town"
                    },
                    Description = "Description Three",
                    IsActive= true,
                    NoOfBeds = 4,
                    PropertyValue = 76547,
                    PurchaseDate = new DateTime(2008, 3, 1),
                    PurchasePrice = 75273,
                    RentalPrice = 654,
                    CreatedDateTime = DateTime.Now.AddDays(-1),
                    Portfolio = portfolio2,
                    Tenants = new List<Tenant>{
                        new Tenant{ FirstName = "FirstName5", LastName = "LastName5", EmailAddress = "test5@test.com", IsActive = true, Profession = "Window cleaner", TenancyStartDate = DateTime.Now.AddDays(-1) },
                    }
                }
            };

            _propertyServiceMock.Setup(s => s.GetPropertiesForPortfolio(It.IsAny<Guid>()))
                .Returns(Task.FromResult<IList<Property>>(properties.Where(x => x.PortfolioId == portfolio1.Id).ToList())).Verifiable();

            var controller = new PropertiesController(_propertyServiceMock.Object, _mapperMock);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = _principle }
            };

            var results = await controller.GetPropertiesByPortfolioId(It.IsAny<Guid>());
            var okResult = results.Result as OkObjectResult;
            var okVal = okResult.Value as IList<PropertyDetailDto>;
            Assert.AreEqual((int)System.Net.HttpStatusCode.OK, okResult.StatusCode);
            Assert.AreEqual(2, okVal.Count());
        }

        [TestMethod()]
        public async Task GetPropertyByIdAsync()
        {
            var portfolio1 = new Portfolio { Name = "Portfolio1" };
            var property = new Property
            {
                Address = new Address
                {
                    City = "Exeter",
                    CreatedDateTime = DateTime.Now.AddDays(-3),
                    IsActive = true,
                    Line1 = "Line 1",
                    Line2 = "Line 2",
                    PostCode = "Postcode",
                    Town = "Town"
                },
                Description = "Description",
                IsActive = true,
                NoOfBeds = 2,
                PropertyValue = 12345,
                PurchaseDate = new DateTime(2008, 3, 1),
                PurchasePrice = 12343,
                RentalPrice = 431,
                CreatedDateTime = DateTime.Now.AddDays(-1),
                Portfolio = portfolio1,
                Tenants = new List<Tenant>{
                        new Tenant{ FirstName = "FirstName1", LastName = "LastName1", EmailAddress = "test1@test.com", IsActive = true, Profession = "Window cleaner", TenancyStartDate = DateTime.Now.AddDays(-1) },
                        new Tenant{ FirstName = "FirstName2", LastName = "LastName2", EmailAddress = "test2@test.com", IsActive = true, Profession = "Superstar DJ", TenancyStartDate = DateTime.Now.AddDays(-1) }
                    }
            };

            _propertyServiceMock.Setup(s => s.GetPropertyById(It.IsAny<Guid>(), It.IsAny<Guid>()))
               .Returns(Task.FromResult<Property>(property)).Verifiable();

            var controller = new PropertiesController(_propertyServiceMock.Object, _mapperMock);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = _principle }
            };

            var results = await controller.GetPropertyById(It.IsAny<Guid>(), It.IsAny<Guid>());
            var okResult = results.Result as OkObjectResult;
            var okVal = okResult.Value as PropertyDetailDto;
            Assert.AreEqual((int)System.Net.HttpStatusCode.OK, okResult.StatusCode);
            Assert.AreEqual("Description", okVal.Description);
            Assert.AreEqual(2, okVal.Tenants.Count);
            Assert.AreEqual(2, okVal.NoOfBeds);
            Assert.AreEqual("Exeter", okVal.Address.City);
        }

        [TestMethod()]
        public void PutPropertyTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public async Task PostPropertyTestAsync()
        {
            var portfolio3 = new Portfolio { Name = "Portfolio3" };
            var property = new Property
            {
                Address = new Address
                {
                    City = "Torquay",
                    CreatedDateTime = DateTime.Now.AddDays(-3),
                    IsActive = true,
                    Line1 = "Line 1",
                    Line2 = "Line 2",
                    PostCode = "Postcode",
                    Town = "Town"
                },
                Description = "Description",
                IsActive = true,
                NoOfBeds = 2,
                PropertyValue = 12345,
                PurchaseDate = new DateTime(2008, 3, 1),
                PurchasePrice = 12343,
                RentalPrice = 431,
                CreatedDateTime = DateTime.Now.AddDays(-1),
                Portfolio = portfolio3,
                Tenants = new List<Tenant>{
                        new Tenant{ FirstName = "Jeff", LastName = "LastName1", EmailAddress = "test1@test.com", IsActive = true, Profession = "Window cleaner", TenancyStartDate = DateTime.Now.AddDays(-1) },
                        new Tenant{ FirstName = "Mandy", LastName = "LastName2", EmailAddress = "test2@test.com", IsActive = true, Profession = "Superstar DJ", TenancyStartDate = DateTime.Now.AddDays(-1) }
                    }
            };

            _propertyServiceMock.Setup(s => s.CreateProperty(property, It.IsAny<Guid>()))
               .Returns(Task.FromResult<Property>(property)).Verifiable();

            var controller = new PropertiesController(_propertyServiceMock.Object, _mapperMock);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = _principle }
            };

            var createProperty = _mapperMock.Map<PropertyCreateDto>(property);
            var results = await controller.CreateProperty(It.IsAny<Guid>(), createProperty);

            var okResult = results.Result as CreatedAtActionResult;
            Assert.AreEqual((int)System.Net.HttpStatusCode.Created, okResult.StatusCode);
        }

        [TestMethod()]
        public void DeletePropertyTest()
        {
            Assert.Fail();
        }
    }
}