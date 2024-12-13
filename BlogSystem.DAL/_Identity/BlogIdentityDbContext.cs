using BlogSystem.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlogSystem.DAL._Identity
{
    public class BlogIdentityDbContext : IdentityDbContext<ApplicationUser>
    {
        public BlogIdentityDbContext(DbContextOptions<BlogIdentityDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


        }
    }
}
