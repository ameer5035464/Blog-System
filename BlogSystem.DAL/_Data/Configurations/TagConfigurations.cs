using BlogSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogSystem.DAL._Data.Configurations
{
    public class TagConfigurations : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.Property(T => T.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(T => T.Name)
                .IsRequired();

            builder.HasIndex(T => T.Name).IsUnique();
        }
    }
}
