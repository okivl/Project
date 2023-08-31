using Microsoft.EntityFrameworkCore;
using Project.Core.Exeptions;
using Project.Core.Interfaces;
using Project.Core.Services;
using Project.Entities;
using Project.Infrastructure.Data;
using Xunit;

namespace Project.UnitTests.Services
{
    public class IncomeSourceServiceTests : IDisposable
    {
        private readonly DataContext _context;
        private readonly IIncomeSourceService _incomeSourceService;

        public IncomeSourceServiceTests()
        {

            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new DataContext(options);

            _incomeSourceService = new IncomeSourceService(_context);

            var incomeSource = new IncomeSource
            {
                Id = Guid.Parse("00000001-0001-0001-0001-000000000001"),
                Name = "Test",
            };
            _context.Add(incomeSource);
            _context.SaveChangesAsync();
        }

        [Fact]
        public async Task Get_ValidReturns_IncomeSource()
        {
            // Arrange

            // Act
            var response = await _incomeSourceService.Get(Guid.Parse("00000001-0001-0001-0001-000000000001"));

            // Assert
            Assert.NotNull(response);
            Assert.IsType<IncomeSource>(response);
            Assert.Equal("Test", response.Name);
        }

        [Fact]
        public async Task Get_NotFoundException()
        {
            // Arrange

            // Act
            var response = _incomeSourceService.Get(Guid.Parse("00000011-0001-0001-0001-000000000001"));

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(() => response);
        }

        [Fact]
        public async Task Update_NotFoundException()
        {
            // Arrange

            // Act
            var response = _incomeSourceService.Update(Guid.Parse("00000011-0001-0001-0001-000000000001"), "");

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
