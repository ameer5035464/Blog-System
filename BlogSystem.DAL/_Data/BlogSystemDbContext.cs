using BlogSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace BlogSystem.DAL._Data
{
    public class BlogSystemDbContext : DbContext
    {
        public BlogSystemDbContext(DbContextOptions<BlogSystemDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly(), type =>
                type.Namespace!.Contains("BlogSystem.DAL._Data.Configurations")
            );


        }

        public DbSet<BlogPost> Posts { get; set; } = null!;
        public DbSet<Tag> Tags { get; set; } = null!;
        public DbSet<Comment> Comments { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<BlogPostTag> BlogPostTag { get; set; } = null!;
    }
}
