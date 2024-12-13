using BlogSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogSystem.DAL._Data.Configurations
{
    public class BlogPostTagConfigurations : IEntityTypeConfiguration<BlogPostTag>
    {
        public void Configure(EntityTypeBuilder<BlogPostTag> builder)
        {
            builder.HasKey(BT => new { BT.TagId, BT.BlogPostId });

            builder.HasOne(B => B.Tag)
                .WithMany(B => B.BlogPostTags)
                .HasForeignKey(B => B.TagId);

            builder.HasOne(B => B.BlogPost)
                .WithMany(B => B.BlogPostTags)
                .HasForeignKey(B => B.BlogPostId);
        }
    }
}
