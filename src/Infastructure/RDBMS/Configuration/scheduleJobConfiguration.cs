using System;
using System.Collections.Generic;
using System.Text;
using Domain.RDBMS.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.RDBMS.Configuration
{
    class ScheduleJobConfiguration : IEntityTypeConfiguration<ScheduleJob>
    {
        public void Configure(EntityTypeBuilder<ScheduleJob> builder)
        {
            builder.ToTable("ScheduleJob");
            builder.Property(e => e.Id)
                .HasColumnName("id");

            builder.Property(e => e.ScheduleId)
                .IsRequired()
                .HasColumnName("scheduleId")
                .HasMaxLength(50);

            builder.Property(e => e.RequestId)
                .IsRequired()
                .HasColumnName("requestId");
        }
    }
}
