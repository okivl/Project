using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
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
    public class AuthServiceTests
    {
        private readonly IAuthService _authService;

        public AuthServiceTests()
        {
            var user = new List<User>
            {
                new User()
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
                }
            };

            var mockContextOptions = new DbContextOptions<DataContext>();

            var mockContext = new Mock<DataContext>(mockContextOptions);
            mockContext.Setup(mc => mc.Users).ReturnsDbSet(user);

            var tokenServiceMock = new Mock<ITokenService>();
            tokenServiceMock.Setup(ts => ts.GenerateAccessToken(It.IsAny<List<Claim>>())).Returns("fakeAccessToken");
            tokenServiceMock.Setup(ts => ts.GenerateRefreshToken()).Returns("fakeRefreshToken");

            _authService = new AuthService(mockContext.Object, tokenServiceMock.Object);
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
                Password = BCryptNet.HashPassword("Regpass123!"),
                PasswordConfirm = BCryptNet.HashPassword("Regpass123!")
            };

            // Act
            var response = await _authService.Registration(regPrarametres);

            // Assert
            Assert.NotNull(response);
            Assert.Equal("fakeAccessToken", response.AccessToken);
            Assert.Equal("fakeRefreshToken", response.RefreshToken);
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
    }
}