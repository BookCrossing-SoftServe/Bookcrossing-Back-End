using Domain.RDBMS.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.RDBMS.Configuration
{
    class NotificationConfiguration: IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("Notification");

            builder.Property(notification => notification.Message)
                .HasMaxLength(512);

            builder.Property(notification => notification.IsRead)
                .HasDefaultValue(false);

            builder.Property(notification => notification.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(notification => notification.Action)
                .HasConversion<int>();

            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(notification => notification.UserId);

            builder.HasOne<Book>()
                .WithMany()
                .HasForeignKey(notification => notification.BookId);
        }
    }
}
