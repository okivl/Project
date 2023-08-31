using Microsoft.EntityFrameworkCore;
using Moq;
using Project.Core.Exeptions;
using Project.Core.Interfaces;
using Project.Core.Models.CreateUpdate;
using Project.Core.Services;
using Project.Entities;
using Project.Infrastructure.Data;
using System.Security.Claims;
using Xunit;
using BCryptNet = BCrypt.Net.BCrypt;

namespace Project.UnitTests.Services
{
    public class AuthServiceTests : IDisposable
    {
        private readonly DataContext _context;
        private readonly IAuthService _authService;

        public AuthServiceTests()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new DataContext(options);

            var tokenServiceMock = new Mock<ITokenService>();
            tokenServiceMock.Setup(ts => ts.GenerateAccessToken(It.IsAny<List<Claim>>())).Returns("fakeAccessToken");
            tokenServiceMock.Setup(ts => ts.GenerateRefreshToken()).Returns("fakeRefreshToken");

            _authService = new AuthService(_context, tokenServiceMock.Object);

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
            _context.Users.Add(user);
            _context.SaveChangesAsync();
        }

        [Fact]
        public async Task Login_ValidReturns_AuthResponse()
        {
            // Arrange

            // Act
            var response = await _authService.Login("test@test.com", "testpass");

            // Assert
            Assert.NotNull(response);
            Assert.Equal("fakeAccessToken", response.AccessToken);
            Assert.Equal("fakeRefreshToken", response.RefreshToken);
        }

        [Fact]
        public async Task Login_NotFoundEcxeption()
        {
            // Arrange

            // Act
            var response = _authService.Login("not_exist@test.com", "testpass");

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(() => response);
        }

        [Fact]
        public async Task Login_WrongPasswordException()
        {
            // Arrange

            // Act
            var response = _authService.Login("test@test.com", "wrongpass");

            //Assert
            await Assert.ThrowsAsync<WrongPasswordException>(() => response);
        }

        [Fact]
        public async Task Registration_ValidReturns_AuthResponse()
        {
            // Arrange

            var regPrarametres = new UserRegParameters
            {
                FirstName = "test",
                Surname = "Test",
                LastName = "Test",
                DateBirth = DateTime.UtcNow,
                Email = "regmail@test.com",
                Login = "regmail@test.com",
                Password = BCryptNet.HashPassword("regpass"),
                PasswordConfirm = BCryptNet.HashPassword("regpass")
            };

            // Act
            var response = await _authService.Registration(regPrarametres);

            // Assert
            Assert.NotNull(response);
            Assert.Equal("fakeAccessToken", response.AccessToken);
            Assert.Equal("fakeRefreshToken", response.RefreshToken);
        }

        [Fact]
        public async Task Registration_AlreadyExistEception()
        {
            // Arrange

            var regPrarametres = new UserRegParameters
            {
                FirstName = "test",
                Surname = "Test",
                LastName = "Test",
                DateBirth = DateTime.UtcNow,
                Email = "test@test.com",
                Login = "test@test.com",
                Password = BCryptNet.HashPassword("regpass"),
                PasswordConfirm = BCryptNet.HashPassword("regpass")
            };

            // Act
            var response = _authService.Registration(regPrarametres);

            // Assert
            await Assert.ThrowsAsync<AlreadyExistException>(() => response);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}