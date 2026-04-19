using Microsoft.AspNetCore.Identity;
using StoreInventorySystem.Domain.Entities;

namespace StoreInventorySystem.Infrastructure
{
    public static class DbSeeder
    {
        public static void SeedAdmin(AppDbContext dbContext)
        {
            if (dbContext.Users.Any(u => u.Username == "admin"))
                return;

            var admin = new User()
            {
                Username = "admin",
                Role = "Admin"
            };

            var hasher = new PasswordHasher<User>();
            admin.PasswordHash = hasher.HashPassword(admin, "super strong admin password");

            dbContext.Users.Add(admin);
            dbContext.SaveChanges();
        }
    }
}
