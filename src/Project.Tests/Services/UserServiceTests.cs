﻿using Microsoft.AspNetCore.Http;
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
    public class UserServiceTests
    {
        private readonly IUserService _userService;

        public UserServiceTests()
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
                RefreshTokenExpiryTime = DateTime.UtcNow,
                }
            };

            var mockContextOptions = new DbContextOptions<DataContext>();

            var mockContext = new Mock<DataContext>(mockContextOptions);
            mockContext.Setup(mc => mc.Users).ReturnsDbSet(user);

            var tokenServiceMock = new Mock<ITokenService>();
            tokenServiceMock.Setup(ts => ts.GenerateAccessToken(It.IsAny<List<Claim>>())).Returns("fakeAccessToken");
            tokenServiceMock.Setup(ts => ts.GenerateRefreshToken()).Returns("fakeRefreshToken");

            var httpContexAccessorMock = new Mock<IHttpContextAccessor>();
            httpContexAccessorMock.Setup(hca => hca.HttpContext.User.Claims)
                .Returns(new List<Claim>
                {
                    new Claim("id", "00000111-0001-0001-0001-000000000001"),
                    new Claim("login", "test@test.com"),
                    new Claim("role", "User")
                });

            _userService = new UserService(mockContext.Object, httpContexAccessorMock.Object, tokenServiceMock.Object);
        }

        [Fact]
        public async Task Get_ValidReturns_User()
        {
            // Arrange

            // Act
            var response = await _userService.Get(Guid.Parse("00000001-0001-0001-0001-000000000001"));

            // Assert
            Assert.NotNull(response);
            Assert.Equal("test@test.com", response.Email);
        }

        [Fact]
        public async Task Get_NotGoundException()
        {
            // Arrange
            // Act
            var response = _userService.Get(Guid.Parse("00000111-0001-0001-0001-000000000001"));

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(() => response);
        }

        [Fact]
        public async Task GetUserInfo_NotFoundException()
        {
            // Arrange

            // Act
            var response = _userService.GetUserInfo();

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(() => response);
        }

        [Fact]
        public async Task Update_NotFoundException()
        {
            // Arrange
            var testupdate = new AdminUserUpdateParameters { };

            // Act
            var response = _userService.Update(Guid.Parse("00000111-0001-0001-0001-000000000001"), testupdate);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(() => response);
        }
    }
}
