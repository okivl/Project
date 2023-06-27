using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Project.Entities.Models;
using Project.Infrastructure.Data;
using BCryptNet = BCrypt.Net.BCrypt;

namespace Project.Core.Options
{
    public static class SeedUser
    {
        public static IHost SeedData(this IHost host)
        {
            var databaseContext = host.Services.CreateScope().ServiceProvider.GetRequiredService<DataContext>();

            var user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "admin",
                Surname = "admin",
                LastName = "admin",
                DateBirth = DateTime.Now,
                Email = "admin",
                Login = "admin",
                Password = BCryptNet.HashPassword("admin"),
                Role = Roles.Admin,
                CreationTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                RefreshToken = "1",
                RefreshTokenExpiryTime = DateTime.Now,
            };
            databaseContext.Add(user);
            databaseContext.SaveChanges();

            return host;
        }
    }
}
