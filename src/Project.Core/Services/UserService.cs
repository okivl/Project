using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Project.Core.Interfaces;
using Project.Core.Models.CreateUpdate;
using Project.Core.Models.Enums;
using Project.Core.Models.SearchContexts;
using Project.Entities;
using Project.Infrastructure.Data;
using BCryptNet = BCrypt.Net.BCrypt;

namespace Project.Core.Services
{
    /// <inheritdoc cref="IUserService"/>
    public class UserService : IUserService
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITokenService _tokenService;

        public UserService(DataContext context, IHttpContextAccessor httpContextAccessor, ITokenService tokenService)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _tokenService = tokenService;
        }

        public async Task<List<User>> GetAll(AdminUserSearchContext searchContext)
        {
            var users = _context.Users.AsQueryable();

            switch (searchContext.Sort)
            {
                case UserSearchSort.None:
                    break;
                case UserSearchSort.Name:
                    users = users.OrderBy(i => i.FirstName);
                    break;
                case UserSearchSort.Surname:
                    users = users.OrderBy(i => i.Surname);
                    break;
                case UserSearchSort.DateBirth:
                    users = users.OrderBy(i => i.DateBirth);
                    break;
                case UserSearchSort.Email:
                    users = users.OrderBy(i => i.Email);
                    break;
            }

            users = searchContext.Order == OrderSort.Ascending ? users : users.Reverse();

            users = users.Skip((searchContext.Page - 1) * searchContext.PageSize).Take(searchContext.PageSize);

            return await users.ToListAsync();
        }

        public async Task<User> Get(Guid Id)
        {
            var user = await _context.Users.FindAsync(Id) ?? throw new Exception("User not found");

            return user;
        }

        public async Task<User> GetUserInfo()
        {
            var httpContext = _httpContextAccessor.HttpContext ?? throw new Exception("No HttpContext");

            var idClaim = httpContext.User.Claims.FirstOrDefault(c => c.Type == "id") ?? throw new Exception("No Claims");

            var user = await _context.Users.FindAsync(Guid.Parse(idClaim.Value)) ?? throw new Exception("User not found");

            return user;
        }

        public async Task Create(AdminUserCreateParameters userCreate)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = userCreate.FirstName,
                Surname = userCreate.Surname,
                LastName = userCreate.LastName,
                DateBirth = userCreate.DateBirth,
                Email = userCreate.Email,
                Login = userCreate.Login,
                Password = BCryptNet.HashPassword(userCreate.Password),
                Role = userCreate.Role,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            user.RefreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(14);

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Guid Id, AdminUserUpdateParameters userUpdate)
        {
            var updateUser = await _context.Users.FindAsync(Id) ?? throw new Exception("User not found");

            updateUser.FirstName = userUpdate.FirstName;
            updateUser.Surname = userUpdate.Surname;
            updateUser.LastName = userUpdate.LastName;
            updateUser.DateBirth = userUpdate.DateBirth;
            updateUser.Email = userUpdate.Email;
            updateUser.Role = userUpdate.Role;
            updateUser.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task UpdateUser(BaseUserUpdateParameters userUpdate)
        {
            var updateUser = await GetUserInfo();

            updateUser.FirstName = userUpdate.FirstName;
            updateUser.Surname = userUpdate.Surname;
            updateUser.LastName = userUpdate.LastName;
            updateUser.DateBirth = userUpdate.DateBirth;
            updateUser.Email = userUpdate.Email;
            updateUser.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task UpdatePassword(Guid Id, BaseUser password)
        {
            var updateUser = await _context.Users.FindAsync(Id) ?? throw new Exception("User not found");

            updateUser.Password = BCryptNet.HashPassword(password.Password);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserPassword(BaseUser password)
        {
            var user = await GetUserInfo();

            user.Password = BCryptNet.HashPassword(password.Password);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Guid Id)
        {
            var user = await _context.Users.FindAsync(Id) ?? throw new Exception("User not found");
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}
