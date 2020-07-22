using System;
using Domain.RDBMS.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.RDBMS.Configuration
{
    class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.ToTable("Book");
            builder.Property(e => e.Id)
                .HasColumnName("id");

            builder.Property(e => e.State)
                .HasMaxLength(50)
                .HasConversion(x => x.ToString(),
                    x => (BookState)Enum.Parse(typeof(BookState), x))
                .HasDefaultValue(BookState.Available);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasColumnName("name")
                .HasMaxLength(150);

            builder.Property(e => e.Rating)
                .HasColumnName("rating")
                .HasDefaultValue(0);

            builder.Property(e => e.Publisher)
                .HasColumnName("publisher")
                .HasMaxLength(150);

            builder.Property(e => e.UserId).HasColumnName("user_id");

            builder.HasOne(d => d.User)
                .WithMany(p => p.Book)
                .HasForeignKey(d => d.UserId);

            builder.Property(e => e.Notice)
                .HasColumnName("notice")
                .HasMaxLength(250);

            builder.Property(a => a.ImagePath)
                .HasColumnName("imagepath")
                .HasMaxLength(260);

        
            builder.HasOne(a => a.Language)
              .WithMany(p => p.Books)
              .HasForeignKey(d => d.LanguageId);



            builder.Property(a => a.DateAdded).HasDefaultValueSql("GETDATE()");
        }
    }
}
