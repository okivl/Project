using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Project.Entities;
using Project.Infrastructure.Data;
using BCryptNet = BCrypt.Net.BCrypt;

namespace Project.Core.Extensions
{
    public static class HostExtension
    {
        public static IHost MigrateDb(this IHost host)
        {
            using var dataContext = host.Services.CreateScope().ServiceProvider.GetRequiredService<DataContext>();

            dataContext.Database.Migrate();

            return host;
        }

        public static IHost SeedData(this IHost host)
        {
            using var databaseContext = host.Services.CreateScope().ServiceProvider.GetRequiredService<DataContext>();

            var id = Guid.Parse("00000001-0001-0001-0001-000000000001");

            var aUser = databaseContext.Users.FirstOrDefault(u => u.Id == id);
            if (aUser != null) return host;

            var user = new User
            {
                Id = id,
                FirstName = "admin",
                Surname = "admin",
                LastName = "admin",
                DateBirth = DateTime.Now,
                Email = "admin",
                Login = "admin",
                Password = BCryptNet.HashPassword("admin"),
                Role = Roles.Admin,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                RefreshToken = "1",
                RefreshTokenExpiryTime = DateTime.Now,
            };
            databaseContext.Add(user);
            databaseContext.SaveChanges();

            return host;
        }
    }
}
