using BlogSystem.DAL.Contracts;
using BlogSystem.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlogSystem.DAL._Identity
{
    public class IdentityIntialize : IDbIdentityIntializer
    {
        private readonly BlogIdentityDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public IdentityIntialize(
            BlogIdentityDbContext dbContext,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task UpdateAllDatabaseAsync()
        {
            var checkAnyMigrations = await _dbContext.Database.GetPendingMigrationsAsync();

            if (checkAnyMigrations.Any())
            {
                await _dbContext.Database.MigrateAsync();
            }
        }

        public async Task SeedDataAsync()
        {
            if (!_roleManager.Roles.Any())
            {
                var roles = new IdentityRole[]
                {
                    new IdentityRole("Admin"),
                    new IdentityRole("Editor"),
                    new IdentityRole("Reader")
                };

                foreach (var role in roles)
                {
                    await _roleManager.CreateAsync(role);
                }
            }
            if (!_userManager.Users.Any())
            {
                var user = new ApplicationUser
                {
                    UserName = "Ameer",
                    Email = "ameerelnagdi503564@gmail.com",
                    PhoneNumber = "01095410698",
                    PictureUrl = "https://res.cloudinary.com/dwvxwl888/image/upload/v1733217205/fgcwfwkpx9ys33rl7gjy.jpg",
                    PicturePublicId = "fgcwfwkpx9ys33rl7gjy"
                };

                await _userManager.CreateAsync(user, "Ameer@12345");
                await _userManager.AddToRoleAsync(user, "Admin");
            }
        }

    }
}
