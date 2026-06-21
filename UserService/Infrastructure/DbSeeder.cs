using Microsoft.AspNetCore.Identity;
using UserService.Domain.Entities;

namespace UserService.Infrastructure
{
    public static class DbSeeder
    {
        public static void SeedAdmins(UserDbContext dbContext)
        {
            var hasher = new PasswordHasher<User>();

            if (!dbContext.Users.Any(u => u.Username == "admin"))
            {
                var admin1 = new User()
                {
                    Username = "admin",
                    Role = "Admin"
                };


                admin1.PasswordHash = hasher.HashPassword(admin1, "super strong admin password");

                dbContext.Users.Add(admin1);
            }

            if (!dbContext.Users.Any(u => u.Username == "another_admin"))
            {

                var admin2 = new User()
                {
                    Username = "another_admin",
                    Role = "Admin"
                };

                admin2.PasswordHash = hasher.HashPassword(admin2, "super strong admin password");

                dbContext.Users.Add(admin2);
            }

            dbContext.SaveChanges();
        }
    }
}
