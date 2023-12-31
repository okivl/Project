﻿using Microsoft.EntityFrameworkCore;
using Project.Core.Exeptions;
using Project.Core.Interfaces;
using Project.Core.Models;
using Project.Core.Models.CreateUpdate;
using Project.Entities;
using Project.Infrastructure.Data;
using System.Security.Claims;
using BCryptNet = BCrypt.Net.BCrypt;

namespace Project.Core.Services
{
    /// <inheritdoc cref="IAuthService"/>
    public class AuthService : IAuthService
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;

        public AuthService(DataContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<AuthResponseDto> Login(string email, string password)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email) ?? throw new NotFoundException();

            if (!BCryptNet.Verify(password, user.Password)) throw new WrongPasswordException();

            var claims = new List<Claim>
            {
                new Claim("id", user.Id.ToString()),
                new Claim("login", user.Email.ToString()),
                new Claim("role", user.Role.ToString())
            };

            var accessToken = _tokenService.GenerateAccessToken(claims);
            user.RefreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(14);
            await _context.SaveChangesAsync();

            var response = new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = user.RefreshToken
            };

            return response;
        }

        public async Task<AuthResponseDto> Registration(UserRegParameters userReg)
        {
            var test = await _context.Users.SingleOrDefaultAsync(u => u.Login == userReg.Login);
            if (test != null) throw new AlreadyExistException();

            var user = new User
            {
                FirstName = userReg.FirstName,
                Surname = userReg.Surname,
                LastName = userReg.LastName,
                DateBirth = userReg.DateBirth,
                Email = userReg.Email,
                Login = userReg.Login,
                Password = BCryptNet.HashPassword(userReg.Password),
                Role = Roles.User,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var claims = new List<Claim>
            {
                new Claim("id", user.Id.ToString()),
                new Claim("login", user.Email.ToString()),
                new Claim("role", user.Role.ToString())
            };

            var accessToken = _tokenService.GenerateAccessToken(claims);
            user.RefreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(14);

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            var response = new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = user.RefreshToken
            };

            return response;
        }

        public async Task<AuthResponseDto> Refresh(string accessToken, string refreshToken)
        {
            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
            var calimId = principal.FindFirst(c => c.Type == "id") ?? throw new InvalidTokenException();

            var id = calimId.Value;
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id.ToString() == id);

            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                throw new InvalidTokenException();
            }

            var claims = new List<Claim>
            {
                new Claim("id", user.Id.ToString()),
                new Claim("login", user.Email.ToString()),
                new Claim("role", user.Role.ToString())
            };

            accessToken = _tokenService.GenerateAccessToken(claims);
            refreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(14);
            await _context.SaveChangesAsync();

            var response = new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,

            };

            return response;
        }
    }
}
