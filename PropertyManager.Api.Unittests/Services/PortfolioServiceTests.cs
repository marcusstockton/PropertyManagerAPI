using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PropertyManagerApi.Data;
using PropertyManagerApi.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PropertyManagerApi.Services.Tests
{
    [TestClass()]
    public class PortfolioServiceTests
    {
        private readonly DbContextOptions<ApplicationDbContext> _options;
        private readonly Mock<ILogger<PortfolioService>> _logger = new Mock<ILogger<PortfolioService>>();

        private Guid user1Id = Guid.NewGuid();
        private Guid user2Id = Guid.NewGuid();
        private Guid portfolio1Id = Guid.NewGuid();
        private Guid portfolio2Id = Guid.NewGuid();

        public PortfolioServiceTests()
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: nameof(PortfolioServiceTests))
                .Options;

            using (var dbContext = new ApplicationDbContext(_options))
            {
                dbContext.Database.EnsureDeleted();
                dbContext.Database.EnsureCreated();
                SetupData(dbContext);
            }
        }

        [TestMethod()]
        public async Task GetPortfolios_Returns_Portfolios_For_Logged_in_user_correctly()
        {
            // Arrange
            using var dbContext = new ApplicationDbContext(_options);
            var portfolioService = new PortfolioService(dbContext, _logger.Object);

            // Act
            var result = await portfolioService.GetPortfolios(user1Id.ToString());

            // Assert
            Assert.AreEqual(1, result.Count());
        }

        [TestMethod()]
        public async Task GetPortfolios_Returns_Empty_For_Invalid_UserId()
        {
            // Arrange
            using var dbContext = new ApplicationDbContext(_options);
            var portfolioService = new PortfolioService(dbContext, _logger.Object);

            // Act
            var result = await portfolioService.GetPortfolios(Guid.NewGuid().ToString());

            // Assert
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod()]
        public async Task GetPortfolioById_For_Valid_Id_Returns_Correct_Portfolio()
        {
            // Arrange
            using var dbContext = new ApplicationDbContext(_options);
            var portfolioService = new PortfolioService(dbContext, _logger.Object);

            // Act
            var result = await portfolioService.GetPortfolioById(portfolio1Id);

            // Assert
            Assert.AreEqual("Test 1", result.Name);
        }

        [TestMethod()]
        public async Task GetPortfolioById_For_Invalid_Id_Returns_Empty()
        {
            // Arrange
            using var dbContext = new ApplicationDbContext(_options);
            var portfolioService = new PortfolioService(dbContext, _logger.Object);

            // Act
            var result = await portfolioService.GetPortfolioById(Guid.NewGuid());

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod()]
        public async Task GetPortfolioAndProperties_With_Valid_PortfolioId_Returns_Correct_PortfolioAndProperties()
        {
            // Arrange
            using var dbContext = new ApplicationDbContext(_options);
            var portfolioService = new PortfolioService(dbContext, _logger.Object);

            // Act
            var result = await portfolioService.GetPortfolioAndProperties(portfolio1Id);

            // Assert
            Assert.AreEqual("Test 1", result.Name);
            Assert.AreEqual(1, result.Properties.Count());
        }

        [TestMethod()]
        public async Task GetPortfolioAndProperties_With_Invalid_PortfolioId_Returns_Correct_PortfolioAndProperties()
        {
            // Arrange
            using var dbContext = new ApplicationDbContext(_options);
            var portfolioService = new PortfolioService(dbContext, _logger.Object);

            // Act
            var result = await portfolioService.GetPortfolioAndProperties(Guid.NewGuid());

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod()]
        public async Task UpdatePortfolio_With_Valid_Data_Correctly_Updates_Portfolio()
        {
            // Arrange
            using var dbContext = new ApplicationDbContext(_options);
            var portfolioService = new PortfolioService(dbContext, _logger.Object);

            // Act
            var portfolio = await portfolioService.GetPortfolioById(portfolio1Id);
            portfolio.Name = "Test Updated Name";
            await portfolioService.UpdatePortfolio(portfolio);
            var result = await portfolioService.GetPortfolioById(portfolio1Id);

            // Assert
            Assert.AreEqual("Test Updated Name", result.Name);
        }

        [TestMethod()]
        public async Task CreatePortfolioTest()
        {
            // Arrange
            using var dbContext = new ApplicationDbContext(_options);
            var portfolioService = new PortfolioService(dbContext, _logger.Object);

            var newPortfolioId = Guid.NewGuid();
            var newPortfolio = new Portfolio
            {
                Id = newPortfolioId,
                Name = "New Portfolio",
                Owner = dbContext.Users.Find(user2Id)
            };

            // Act
            var result = await portfolioService.CreatePortfolio(newPortfolio, user2Id);

            // Assert
            Assert.AreEqual(newPortfolioId, result.Id);
            Assert.AreEqual("New Portfolio", result.Name);
        }

        [TestMethod()]
        public async Task DeletePortfolioTest()
        {
            // Arrange
            using var dbContext = new ApplicationDbContext(_options);
            var portfolioService = new PortfolioService(dbContext, _logger.Object);

            // Act
            var result = await portfolioService.DeletePortfolio(portfolio2Id);

            // Assert
            Assert.IsTrue(result);
            var portfolio = await portfolioService.GetPortfolioById(portfolio2Id);
            Assert.IsNull(portfolio);
        }

        private void SetupData(ApplicationDbContext dbContext)
        {
            var user1 = new ApplicationUser
            {
                Id = user1Id,
                UserName = "TestUser@test.com",
                Email = "TestUser@test.com",
            };

            var user2 = new ApplicationUser
            {
                Id = user2Id,
                UserName = "TestUser2@test.com",
                Email = "TestUser2@test.com",
            };

            var portfolio1 = new Portfolio
            {
                CreatedDateTime = DateTime.Now,
                Id = portfolio1Id,
                IsActive = true,
                Name = "Test 1",
                Owner = user1,
                Properties = new System.Collections.Generic.List<Property> { new Property
                {
                    Address = new Address
                    {
                        Id = Guid.NewGuid(),
                        CreatedDateTime = DateTime.Now,
                        City = "Exeter",
                        Line1 = "14",
                        Line2 = "Westaways",
                        PostCode = "EX1 1EX",
                        IsActive = true
                    },
                    IsActive = true,
                    Description = "Cute 1 bed flat in the heart of the city",
                    NoOfBeds = 1,
                    PropertyValue = 120000,
                    PurchaseDate = new DateTime(2018, 1, 3),
                    PurchasePrice = 82000,
                    RentalPrice = 1000,
                    Tenants = new System.Collections.Generic.List<Tenant>{new Tenant
                    {
                        ContactNumber = "019283747521",
                        CreatedDateTime = DateTime.Now,
                        EmailAddress = "foo@bar.com",
                        FirstName = "Test",
                        LastName = "Tenant",
                        Id = Guid.NewGuid(),
                        IsActive = true,
                        Profession = "Juggler",
                        TenancyStartDate = new DateTime(2019,3,6),
                        Title = "Mr",
                        Notes = new System.Collections.Generic.List<Note>
                        {
                            new Note
                            {
                                Title = "Test Note",
                                IsActive = true,
                                CreatedDateTime = DateTime.Now,
                                Description = "Some test data here",
                                Id = Guid.NewGuid()
                            }
                        }
                    } }
                } },
                UpdatedDateTime = DateTime.Now,
            };

            var portfolio2 = new Portfolio
            {
                Id = portfolio2Id,
                Name = "Test 2",
                Owner = user2
            };

            dbContext.Portfolios.AddRange(portfolio1, portfolio2);
            dbContext.SaveChanges();
        }
    }
}