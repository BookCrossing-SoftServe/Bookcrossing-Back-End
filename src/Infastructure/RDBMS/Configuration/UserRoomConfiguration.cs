using Domain.RDBMS.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.RDBMS.Configuration
{
    class UserRoomConfiguration : IEntityTypeConfiguration<UserRoom>
    {
        public void Configure(EntityTypeBuilder<UserRoom> builder)
        {
            builder.ToTable("UserRoom");
            builder.Property(e => e.Id)
                .HasColumnName("id");

            builder.Property(e => e.LocationId).HasColumnName("location_id");

            builder.Property(e => e.RoomNumber).HasColumnName("room_number")
                .HasMaxLength(7);

            builder.HasOne(d => d.Location)
                .WithMany(p => p.UserRoom)
                .HasForeignKey(d => d.LocationId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasMany(d => d.User)
                .WithOne(p => p.UserRoom)
                .HasForeignKey(d => d.UserRoomId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }       
    }
}
