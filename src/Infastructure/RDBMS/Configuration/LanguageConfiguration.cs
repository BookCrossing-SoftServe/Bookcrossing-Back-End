using Domain.RDBMS.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.RDBMS.Configuration
{
    class LanguageConfiguration: IEntityTypeConfiguration<Language>
    {
        public void Configure(EntityTypeBuilder<Language> builder)
        {
            builder.ToTable("Language");
            builder.Property(e => e.Id)
                .HasColumnName("id");

            builder.Property(e => e.Name)
                .IsRequired()
                .HasColumnName("name")
                .HasMaxLength(20);
        }
    }
}

