using System;
using System.Collections.Generic;
using System.Text;
using Domain.RDBMS.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.RDBMS.Configuration
{
    class WishConfiguration : IEntityTypeConfiguration<Wish>
    {
        public void Configure(EntityTypeBuilder<Wish> builder)
        {
            builder.ToTable("Wish");

            builder.HasKey(wish => new { wish.UserId, wish.BookId });

            builder.Property(wish => wish.UserId).HasColumnName("user_id");
            builder.Property(wish => wish.BookId).HasColumnName("book_id");

            builder.HasOne(wish => wish.User)
                .WithMany(user => user.Wish)
                .HasForeignKey(wish => wish.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(wish => wish.Book)
                .WithMany()
                .HasForeignKey(wish => wish.BookId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
