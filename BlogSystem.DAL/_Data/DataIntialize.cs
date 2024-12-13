using BlogSystem.DAL.Contracts;
using BlogSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlogSystem.DAL._Data
{
    public class DataIntialize : IDbDataIntializer
    {
        private readonly BlogSystemDbContext _dbContext;

        public DataIntialize(BlogSystemDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task UpdateAllDatabaseAsync()
        {
            var checkForMigrations = await _dbContext.Database.GetPendingMigrationsAsync();

            if (checkForMigrations.Any())
            {
                await _dbContext.Database.MigrateAsync();
            }
        }

        public async Task SeedDataAsync()
        {
            if (!_dbContext.Categories.Any())
            {
                var category = new Category
                {
                    Name = "Travels"
                };

                await _dbContext.Categories.AddAsync(category);
                await _dbContext.SaveChangesAsync();
            }

            if (!_dbContext.Posts.Any())
            {
                var post = new BlogPost
                {
                    AuthorId = "4ad6e944-b158-408e-9af1-dce62d5a9912",
                    CategoryId = 1,
                    Content = "Traveling can be an exciting and rewarding experience, but it often comes with challenges that can make the trip less enjoyable. From flight delays to language barriers, unexpected situations can test even the most seasoned travelers. To help you make the most of your next adventure, we’ve compiled 10 essential travel tips that will ensure a smoother, stress-free journey.",
                    Title = "Travel Tips",
                };

                await _dbContext.Posts.AddAsync(post);
                await _dbContext.SaveChangesAsync();

            }
        }
    }
}
