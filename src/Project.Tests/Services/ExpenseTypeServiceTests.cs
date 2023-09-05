using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using Project.Core.Exeptions;
using Project.Core.Interfaces;
using Project.Core.Services;
using Project.Entities;
using Project.Infrastructure.Data;
using Xunit;

namespace Project.UnitTests.Services
{
    public class ExpenseTypeServiceTests
    {
        private readonly IExpenseTypeService _expenseTypeService;

        public ExpenseTypeServiceTests()
        {
            var expenseType = new List<ExpenseType>
            {
                new ExpenseType
                {
                Id = Guid.Parse("00000001-0001-0001-0001-000000000001"),
                Name = "Test"
                }
            };

            var mockContextOptions = new DbContextOptions<DataContext>();

            var mockContext = new Mock<DataContext>(mockContextOptions);
            mockContext.SetupGet(mc => mc.ExpenseTypes).ReturnsDbSet(expenseType);

            _expenseTypeService = new ExpenseTypeService(mockContext.Object);
        }

        [Fact]
        public async Task Get_ValidReturns_ExpenseType()
        {
            // Arrange

            // Act
            var response = await _expenseTypeService.Get(Guid.Parse("00000001-0001-0001-0001-000000000001"));

            // Assert
            Assert.NotNull(response);
            Assert.IsType<ExpenseType>(response);
            Assert.Equal("Test", response.Name);
        }

        [Fact]
        public async Task Get_NotFoundException()
        {
            // Arrange

            // Act
            var response = _expenseTypeService.Get(Guid.Parse("00000011-0001-0001-0001-000000000001"));

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(() => response);
        }

        [Fact]
        public async Task Update_NotFoundException()
        {
            // Arrange

            // Act
            var response = _expenseTypeService.Update(Guid.Parse("00000011-0001-0001-0001-000000000001"), "");

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(() => response);
        }
    }
}
