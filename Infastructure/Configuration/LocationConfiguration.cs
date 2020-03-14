using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration
{
    class LocationConfiguration : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> builder)
        {
            builder.ToTable("Location");
            builder.Property(e => e.Id)
                .HasColumnName("id")
                .ValueGeneratedNever();

            builder.Property(e => e.City)
                .IsRequired()
                .HasColumnName("city")
                .HasMaxLength(30);

            builder.Property(e => e.OfficeName)
                .HasColumnName("office_name")
                .HasMaxLength(10);

            builder.Property(e => e.Street)
                .IsRequired()
                .HasColumnName("street")
                .HasMaxLength(50);
        }
    }
}
