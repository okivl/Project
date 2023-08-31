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
    public class ExpenseServiceTests : IDisposable
    {
        private readonly DataContext _context;
        private readonly IExpenseService _expenseService;

        public ExpenseServiceTests()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new DataContext(options);

            var userServiceMock = new Mock<IUserService>();

            _expenseService = new ExpenseService(userServiceMock.Object, _context);
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

            var type = new ExpenseType
            {
                Id = Guid.Parse("00000001-0001-0001-0001-000000000001"),
                Name = "Test",
            };

            var expense = new Expense
            {
                Id = Guid.Parse("00000001-0001-0001-0001-000000000001"),
                Name = "Test",
                Amount = 1,
                ExpenseType = type,
                User = user,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            _context.Add(expense);
            _context.SaveChangesAsync();
        }

        [Fact]
        public async Task Get_ValidReturns_Expense()
        {
            // Arrange

            // Act
            var response = await _expenseService.Get(Guid.Parse("00000001-0001-0001-0001-000000000001"));

            // Assert
            Assert.NotNull(response);
            Assert.IsType<Expense>(response);
            Assert.Equal("Test", response.Name);
        }

        [Fact]
        public async Task Get_NotFoundException()
        {
            // Arrange

            // Act
            var response = _expenseService.Get(Guid.Parse("00000011-0001-0001-0001-000000000001"));

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
