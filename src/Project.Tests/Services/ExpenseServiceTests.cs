using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using Project.Core.Exeptions;
using Project.Core.Interfaces;
using Project.Core.Services;
using Project.Entities;
using Project.Infrastructure.Data;
using Xunit;
using BCryptNet = BCrypt.Net.BCrypt;

namespace Project.UnitTests.Services
{
    public class ExpenseServiceTests
    {
        private readonly IExpenseService _expenseService;

        public ExpenseServiceTests()
        {
            var user = new List<User>
            {
                new User
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
                RefreshTokenExpiryTime = DateTime.UtcNow
                }
            };

            var type = new List<ExpenseType>
            {
                new ExpenseType
                {
                Id = Guid.Parse("00000001-0001-0001-0001-000000000001"),
                Name = "Test"
                }
            };

            var expense = new List<Expense>
            {
                new Expense
                {
                Id = Guid.Parse("00000001-0001-0001-0001-000000000001"),
                Name = "Test",
                Amount = 1,
                ExpenseType = type[0],
                User = user[0],
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
                }
            };

            var mockContextOptions = new DbContextOptions<DataContext>();

            var mockContext = new Mock<DataContext>(mockContextOptions);
            mockContext.Setup(mc => mc.Users).ReturnsDbSet(user);
            mockContext.Setup(mc => mc.ExpenseTypes).ReturnsDbSet(type);
            mockContext.Setup(mc => mc.Expenses).ReturnsDbSet(expense);

            var userServiceMock = new Mock<IUserService>();

            _expenseService = new ExpenseService(userServiceMock.Object, mockContext.Object);
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
    }
}
