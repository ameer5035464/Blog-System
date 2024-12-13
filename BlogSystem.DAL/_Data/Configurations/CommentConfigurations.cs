using BlogSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogSystem.DAL._Data.Configurations
{
    public class CommentConfigurations : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.Property(C => C.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(C => C.AuthorId)
                .IsRequired();

            builder.Property(C => C.Content)
                .HasColumnType("varchar(max)")
                .IsRequired();

            builder.Property(C => C.CreatedAt)
                .HasDefaultValueSql("SYSDATETIMEOFFSET()")
                .ValueGeneratedOnAdd();

            builder.Property(B => B.UpdatedAt)
                .IsRequired(false);

            builder.HasOne(C => C.Post)
                .WithMany()
                .HasForeignKey(C => C.PostId);
        }
    }
}
