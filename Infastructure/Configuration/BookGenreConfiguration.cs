using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration
{
    class BookGenreConfiguration : IEntityTypeConfiguration<BookGenre>
    {
        public void Configure(EntityTypeBuilder<BookGenre> builder)
        {
            builder.ToTable("BookGenre");
            builder.HasKey(e => new { e.BookId, e.GenreId })
                .HasName("PK_BookGenre_id");

            builder.Property(e => e.BookId).HasColumnName("book_id");

            builder.Property(e => e.GenreId).HasColumnName("genre_id");

            builder.HasOne(d => d.Book)
                .WithMany(p => p.BookGenre)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BookGenre_Book");

            builder.HasOne(d => d.Genre)
                .WithMany(p => p.BookGenre)
                .HasForeignKey(d => d.GenreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BookGenre_Genre");
        }
    }
}
