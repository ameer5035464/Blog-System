using BlogSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.DAL._Data.Configurations
{
    public class CategoryConfigurations : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(C => C.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(C => C.Name)
                .IsRequired();

            builder.HasIndex(T => T.Name).IsUnique();
        }
    }
}
