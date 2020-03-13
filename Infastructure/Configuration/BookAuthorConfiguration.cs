using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration
{
    class BookAuthorConfiguration : IEntityTypeConfiguration<BookAuthor>
    {
        public void Configure(EntityTypeBuilder<BookAuthor> builder)
        {
            builder.ToTable("BookAuthor");
            builder.HasKey(e => new { e.BookId, e.AuthorId })
                .HasName("PK_BookAuthor_id");

            builder.Property(e => e.BookId).HasColumnName("book_id");

            builder.Property(e => e.AuthorId).HasColumnName("author_id");

            builder.HasOne(d => d.Author)
                .WithMany(p => p.BookAuthor)
                .HasForeignKey(d => d.AuthorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BookAuthor_Author");

            builder.HasOne(d => d.Book)
                .WithMany(p => p.BookAuthor)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BookAuthor_Book");
        }
    }
}
