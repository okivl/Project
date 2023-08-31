using Microsoft.EntityFrameworkCore;
using Moq;
using Project.Core.Exeptions;
using Project.Core.Interfaces;
using Project.Core.Services;
using Project.Entities;
using Project.Infrastructure.Data;
using Xunit;
using BCryptNet = BCrypt.Net.BCrypt;

namespace Project.UnitTests.Services
{
    public class IncomeServiceTests : IDisposable
    {
        private readonly DataContext _context;
        private readonly IIncomeService _incomeService;

        public IncomeServiceTests()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new DataContext(options);

            var userServiceMock = new Mock<IUserService>();

            _incomeService = new IncomeService(userServiceMock.Object, _context);

            var user = new User
            {
                Id = Guid.Parse("00000001-0001-0001-0001-000000000001"),
                FirstName = "Test",
                Surname = "Test",
                LastName = "Test",
                DateBirth = DateTime.UtcNow,
                Email = "test@test.com",
                Login = "test@test.com",
                Password = BCryptNet.HashPassword("testpass"),
                Role = Roles.User,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                RefreshToken = "",
                RefreshTokenExpiryTime = DateTime.UtcNow,
            };

            var source = new IncomeSource
            {
                Id = Guid.Parse("00000001-0001-0001-0001-000000000001"),
                Name = "Test",
            };

            var income = new Income
            {
                Id = Guid.Parse("00000001-0001-0001-0001-000000000001"),
                Name = "Test",
                Amount = 1,
                IncomeSource = source,
                User = user,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            _context.Add(income);
            _context.SaveChangesAsync();
        }

        [Fact]
        public async Task Get_ValidReturns_Income()
        {
            // Arrange

            // Act
            var response = await _incomeService.Get(Guid.Parse("00000001-0001-0001-0001-000000000001"));

            // Assert
            Assert.NotNull(response);
            Assert.IsType<Income>(response);
            Assert.Equal("Test", response.Name);
        }

        [Fact]
        public async Task Get_NotFoundException()
        {
            // Arrange

            // Act
            var response = _incomeService.Get(Guid.Parse("00000011-0001-0001-0001-000000000001"));

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(() => response);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
