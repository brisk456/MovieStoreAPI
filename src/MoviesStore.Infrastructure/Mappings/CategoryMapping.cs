using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoviesStore.Domain.Models;

namespace MoviesStore.Infrastructure.Mappings
{
    public class CategoryMapping : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasColumnType("varchar(150)");

            builder.HasMany(x => x.Movies)
                .WithOne(x => x.Category)
                .HasForeignKey(x => x.CategoryId);

            builder.ToTable("Category");
        }
    }
}
