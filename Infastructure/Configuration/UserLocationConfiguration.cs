using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Configuration
{
    class UserLocationConfiguration : IEntityTypeConfiguration<UserLocation>
    {
        public void Configure(EntityTypeBuilder<UserLocation> builder)
        {
            builder.ToTable("UserLocation");
            builder.HasKey(e => new {e.UserId, e.LocationId});

            builder.Property(e => e.UserId).HasColumnName("user_id");

            builder.Property(e => e.LocationId).HasColumnName("location_id");

            builder.Property(e => e.RoomNumber).HasColumnName("room_number");

            builder.HasOne(d => d.Location)
                .WithMany(p => p.UserLocation)
                .HasForeignKey(d => d.LocationId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(d => d.User)
                .WithMany(p => p.UserLocation)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }       
    }
}
