using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Moq;
using PropertyManagerApi.Interfaces;
using AutoMapper;
using System.Security.Claims;
using System.Security.Principal;
using PropertyManagerApi.Models;
using PropertyManagerApi.Models.DTOs.Property;

namespace PropertyManagerApi.Controllers.Tests
{
    [TestClass()]
    public class PropertiesControllerTests
    {
        private readonly Mock<IPropertyService> _propertyServiceMock;
        private readonly Mock<IMapper> _mapperMock;
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

            _mapperMock.Setup(m => m.Map<Property, PropertyDetailDto>(It.IsAny<Property>())).Returns(new PropertyDetailDto());
            _mapperMock.Setup(m => m.Map<Property, PropertyCreateDto>(It.IsAny<Property>())).Returns(new PropertyCreateDto());
        }


        [TestMethod()]
        public void GetPropertiesByPortfolioIdTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetPropertyTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetPropertyDetailsTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void PutPropertyTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void PostPropertyTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DeletePropertyTest()
        {
            Assert.Fail();
        }
    }
}