using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PropertyManager.Api.Models.DTOs.Portfolio;
using PropertyManager.Api.Profiles;
using PropertyManagerApi.Interfaces;
using PropertyManagerApi.Models;
using PropertyManagerApi.Models.DTOs.Portfolio;
using PropertyManagerApi.Profiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace PropertyManagerApi.Controllers.Tests
{
    [TestClass()]
    public class PortfoliosControllerTests
    {
        private Mock<IPortfolioServce> _portfolioServceMock;
        private IMapper _mapperMock;
        private GenericIdentity _identity;
        private ApplicationUser _user;
        private ApplicationUser _user2;
        private ClaimsPrincipal _principle;

        public PortfoliosControllerTests()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new PortfolioProfile());
                mc.AddProfile(new PropertyProfile());
                mc.AddProfile(new UserProfile());
                mc.AddProfile(new TenantProfile());
                mc.AddProfile(new AddressProfile());
            });

            _mapperMock = new Mapper(mappingConfig);

            _portfolioServceMock = new Mock<IPortfolioServce>();
        }

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
        }

        [TestMethod()]
        public async Task Get_List_of_Portfolios_For_User_Returns_Correct_Data()
        {
            var portfolioList = new List<Portfolio> {
                new Portfolio
                {
                    Id = Guid.NewGuid(), CreatedDateTime = DateTime.Now, IsActive = true, Name = "Portfolio 1", Owner = _user
                },
                new Portfolio
                {
                    Id= Guid.NewGuid(), CreatedDateTime = DateTime.Now.AddMonths(-3), IsActive = true, Name = "Portfolio 2", Owner = _user2
                }
            };

            _portfolioServceMock.Setup(s => s.GetPortfolios(It.IsAny<string>()))
                .Returns(Task.FromResult<IEnumerable<Portfolio>>(portfolioList.Where(x => x.Owner == _user))).Verifiable();

            var controller = new PortfoliosController(_portfolioServceMock.Object, _mapperMock);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = _principle }
            };

            var results = await controller.GetPortfolios();
            var okResult = results.Result as OkObjectResult;
            var okVal = okResult.Value as IEnumerable<PortfolioListItemDto>;
            Assert.AreEqual((int)System.Net.HttpStatusCode.OK, okResult.StatusCode);
            Assert.AreEqual(1, okVal.Count());
        }

        [TestMethod()]
        public async Task Get_Portfolio_By_Id_Returns_Correct_Portfolio()
        {
            var portfolio = new Portfolio { Id = Guid.NewGuid(), CreatedDateTime = DateTime.Now, Name = "Test", Owner = _user, IsActive = true };
            _portfolioServceMock.Setup(x => x.GetPortfolioById(It.IsAny<Guid>())).ReturnsAsync(portfolio);

            var controller = new PortfoliosController(_portfolioServceMock.Object, _mapperMock);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = _principle }
            };
            var results = await controller.GetPortfolioById(It.IsAny<Guid>());
            Assert.IsNotNull(results);
            var okResult = results.Result as OkObjectResult;
            Assert.AreEqual((int)HttpStatusCode.OK, okResult.StatusCode);

            var okVal = okResult.Value as PortfolioDetailDto;
        }

        [TestMethod()]
        public async Task GetPortfolioAndProperties_Returns_The_Correct_Portfolio_And_Related_PropertiesAsync()
        {
            var portfolio = new Portfolio {
                Id = Guid.NewGuid(),
                CreatedDateTime = DateTime.Now,
                Name = "Test",
                Owner = _user,
                IsActive = true,
                Properties = new List<Property>
                {
                    new Property {
                        Address = new Address
                        {
                            City = "Exeter",
                            CreatedDateTime = DateTime.Now,
                            IsActive = true,
                            Line1 = "Line 1",
                            PostCode = "EX1 1EX"
                        },
                        Description = "Some Description",
                        IsActive = true,
                        NoOfBeds = 2,
                        PropertyValue = 12345,
                        RentalPrice = 432,
                        Tenants = new List<Tenant>
                        {
                            new Tenant
                            {
                                FirstName = "Bob",
                                LastName = "Builder",
                                ContactNumber = "019238475203",
                                EmailAddress = "test@test.com",
                                IsActive = true,
                                Profession = "Builder",
                                TenancyStartDate = DateTime.Now.AddDays(-90),
                                Title = "Mr"
                            }
                        },
                        PurchaseDate = DateTime.Now.AddDays(-96),
                        PurchasePrice = 908734
                    }
                } 
            };
            _portfolioServceMock.Setup(x => x.GetPortfolioAndProperties(It.IsAny<Guid>())).ReturnsAsync(portfolio);
            
            var controller = new PortfoliosController(_portfolioServceMock.Object, _mapperMock);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = _principle }
            };
            var results = await controller.GetPortfolioAndProperties(It.IsAny<Guid>());
            Assert.IsNotNull(results);
            var okResult = results.Result as OkObjectResult;
            Assert.AreEqual((int)HttpStatusCode.OK, okResult.StatusCode);

            var okVal = okResult.Value as PortfolioDetailDto;
            Assert.AreEqual(1, okVal.Properties.Count);
            Assert.AreEqual(1, okVal.Properties.First().Tenants.Count);
        }

        [TestMethod()]
        public void Updating_Portfolio_Correctly_Updates_Portfolio()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public async Task Create_Portfolio_With_Valid_Data_WorksAsync()
        {
            var portfolio = new Portfolio { Name = "New Portfolio", Owner = _user };

            _portfolioServceMock.Setup(x => x.CreatePortfolio(portfolio, _user.Id)).ReturnsAsync(portfolio);

            var controller = new PortfoliosController(_portfolioServceMock.Object, _mapperMock);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = _principle }
            };

            var createPortfolio = _mapperMock.Map<PortfolioCreateDto>(portfolio);
            var results = await controller.CreatePortfolio(createPortfolio);

            var okResult = results.Result as CreatedAtActionResult;
            Assert.AreEqual((int)HttpStatusCode.Created, okResult.StatusCode);
        }

        [TestMethod()]
        public void DeletePortfolio_Correctly_Deletes_The_Portfolio()
        {
            Assert.Fail();
        }
    }
}