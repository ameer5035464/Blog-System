using BlogSystem.DAL.Entities;
using BlogSystem.DAL.Entities.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogSystem.DAL._Data.Configurations
{
    public class BlogPostConfigurations : IEntityTypeConfiguration<BlogPost>
    {
        public void Configure(EntityTypeBuilder<BlogPost> builder)
        {
            builder.Property(B => B.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.Property(B => B.Title)
                .IsRequired();

            builder.Property(B => B.Content)
                .HasColumnType("varchar(max)")
                .IsRequired();

            builder.Property(B => B.AuthorId)
               .IsRequired();

            builder.Property(B => B.CreatedAt)
              .HasDefaultValueSql("SYSDATETIMEOFFSET()")
               .ValueGeneratedOnAdd();

            builder.Property(B => B.UpdatedAt)
                .IsRequired(false);

            builder.Property(B => B.Status)
                .HasConversion(S => S.ToString(), G => (Status)Enum.Parse(typeof(Status), G!));

            builder.HasOne(B => B.Category)
                .WithMany()
                .HasForeignKey(B => B.CategoryId);
        }
    }
}
