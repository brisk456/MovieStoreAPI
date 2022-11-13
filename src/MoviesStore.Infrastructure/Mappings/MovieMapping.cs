using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoviesStore.Domain.Models;

namespace MoviesStore.Infrastructure.Mappings
{
    public class MovieMapping : IEntityTypeConfiguration<Movie>
    {
        public void Configure(EntityTypeBuilder<Movie> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title)
                .IsRequired()
                .HasColumnType("varchar(150)");

            builder.Property(x => x.Author)
                .IsRequired()
                .HasColumnType("varchar(150)");

            builder.Property(x => x.Description)
                .IsRequired(false)
                .HasColumnType("varchar(350)");

            builder.Property(x => x.ReleaseDate)
                .IsRequired();

            builder.Property(x => x.CategoryId)
                .IsRequired();

            builder.ToTable("Movies");
        }
    }
}
