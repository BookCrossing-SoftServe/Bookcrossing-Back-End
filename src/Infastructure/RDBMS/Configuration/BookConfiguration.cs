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

            builder.Property(e => e.Available).HasColumnName("available");

            builder.Property(e => e.Name)
                .IsRequired()
                .HasColumnName("name")
                .HasMaxLength(50);

            builder.Property(e => e.Publisher)
                .HasColumnName("publisher")
                .HasMaxLength(50);

            builder.Property(e => e.UserId).HasColumnName("user_id");

            builder.HasOne(d => d.User)
                .WithMany(p => p.Book)
                .HasForeignKey(d => d.UserId);
        }
    }
}
