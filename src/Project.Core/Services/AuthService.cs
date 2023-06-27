using Microsoft.EntityFrameworkCore;
using Project.Core.Interfaces;
using Project.Core.Options;
using Project.Core.Options.Params.CreateUpdate;
using Project.Entities.Models;
using Project.Infrastructure.Data;
using System.Security.Claims;
using BCryptNet = BCrypt.Net.BCrypt;

namespace Project.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;

        public AuthService(DataContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<AuthResp> Login(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null || !BCryptNet.Verify(password, user.Password)) throw new Exception("Wrong pass");

            var claims = new List<Claim>
            {
                new Claim("id", user.Id.ToString()),
                new Claim("login", email.ToString()),
                new Claim("role", user.Role.ToString())
            };

            var accessToken = _tokenService.GenerateAccessToken(claims);
            user.RefreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(14);
            await _context.SaveChangesAsync();

            var response = new AuthResp
            {
                AccessToken = accessToken,
                RefreshToken = user.RefreshToken
            };

            return response;
        }

        public async Task<AuthResp> Registr(UserReg userReg)
        {
            var test = await _context.Users.FirstOrDefaultAsync(u => u.Login == userReg.Login);
            if (test != null) throw new Exception("User already exist");

            if (userReg.Password != userReg.PasswordConfirm) throw new Exception("Passwords don't match");

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
                CreationTime = DateTime.UtcNow,
                UpdateTime = DateTime.UtcNow
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

            var response = new AuthResp
            {
                AccessToken = accessToken,
                RefreshToken = user.RefreshToken
            };

            return response;
        }

        public async Task<string> Refresh(string accessToken, string refreshToken)
        {
            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
            var calimId = principal.FindFirst(c => c.Type == "id") ?? throw new Exception("Invalid access token");

            var id = calimId.Value;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id.ToString() == id);

            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                throw new Exception("Invalid access token or refresh token");
            }

            var claims = new List<Claim>
            {
                new Claim("id", user.Id.ToString()),
                new Claim("login", user.Email.ToString()),
                new Claim("role", user.Role.ToString())
            };

            accessToken = _tokenService.GenerateAccessToken(claims);
            user.RefreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(14);
            await _context.SaveChangesAsync();

            return accessToken;
        }
    }
}
