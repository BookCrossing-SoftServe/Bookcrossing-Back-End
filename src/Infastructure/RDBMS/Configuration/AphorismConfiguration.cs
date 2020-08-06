using Domain.RDBMS.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.RDBMS.Configuration
{
    class AphorismConfiguration : IEntityTypeConfiguration<Aphorism>
    {
        public void Configure(EntityTypeBuilder<Aphorism> builder)
        {
            builder.ToTable("Aphorism");
            builder.Property(e => e.Id)
                .HasColumnName("id");
            builder.Property(e => e.Phrase)
                .HasColumnName("phrase");
            builder.Property(e => e.PhraseAuthor)
                .IsRequired()
                .HasColumnName("phraseAuthor");
        }
    }
}
