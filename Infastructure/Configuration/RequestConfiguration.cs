using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration
{
    class RequestConfiguration : IEntityTypeConfiguration<Request>
    {
        public void Configure(EntityTypeBuilder<Request> builder)
        {
            builder.ToTable("Request");
            builder.Property(e => e.Id)
                .HasColumnName("id");

            builder.Property(e => e.BookId).HasColumnName("book_id");

            builder.Property(e => e.OwnerId).HasColumnName("owner_id");

            builder.Property(e => e.ReceiveDate)
                .HasColumnName("receive_date")
                .HasColumnType("date");

            builder.Property(e => e.RequestDate)
                .HasColumnName("request_date")
                .HasColumnType("date");

            builder.Property(e => e.UserId).HasColumnName("user_id");

            builder.HasOne(d => d.Book)
                .WithMany(p => p.Request)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(d => d.Owner)
                .WithMany(p => p.RequestOwner)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(d => d.User)
                .WithMany(p => p.RequestUser)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
