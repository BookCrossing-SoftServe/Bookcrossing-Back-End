using System;
using Domain.RDBMS.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.RDBMS.Seeder
{
    public static class DataSeeder
    {
        public static void Seed(ModelBuilder builder)
        {
            Seed(builder.Entity<Author>());
            Seed(builder.Entity<Genre>());
            Seed(builder.Entity<Language>());
            Seed(builder.Entity<Location>());
            Seed(builder.Entity<UserRoom>());
            Seed(builder.Entity<Role>());
            Seed(builder.Entity<User>());
            Seed(builder.Entity<Book>());
            Seed(builder.Entity<BookGenre>());
            Seed(builder.Entity<BookAuthor>());
        }

        private static void Seed(EntityTypeBuilder<Author> builder)
        {
            builder.HasData(
                new Author
                {
                    Id = 1,
                    FirstName = "Bernard",
                    LastName = "Fernandez",
                    IsConfirmed = true
                }
            );
        }

        private static void Seed(EntityTypeBuilder<Genre> builder)
        {
            builder.HasData(
                new Genre
                {
                    Id = 1,
                    Name = "Fantasy"
                }
            );
        }

        private static void Seed(EntityTypeBuilder<Language> builder)
        {
            builder.HasData(
                new Language
                {
                    Id = 1,
                    Name = "Ukrainian"
                }
            );
        }

        private static void Seed(EntityTypeBuilder<Location> builder)
        {
            builder.HasData(
                new Location
                {
                    Id = 1,
                    City = "Lviv",
                    Street = "Gorodoc'kogo",
                    OfficeName = "SoftServe",
                    IsActive = true
                }
            );
        }

        private static void Seed(EntityTypeBuilder<UserRoom> builder)
        {
            builder.HasData(
                new UserRoom
                {
                    Id = 1,
                    LocationId = 1,
                    RoomNumber = "4040",
                }
            );
        }

        private static void Seed(EntityTypeBuilder<Role> builder)
        {
            builder.HasData(
                new Role
                {
                    Id = 1,
                    Name = "User"
                },
                new Role
                {
                    Id = 2,
                    Name = "Admin"
                }
            );
        }

        private static void Seed(EntityTypeBuilder<User> builder)
        {
            builder.HasData(
                new User
                {
                    Id = 1,
                    FirstName = "Admin",
                    MiddleName = "Adminovski",
                    LastName = "Adminovich",
                    Email = "admin@gmail.com",
                    Password = "admin",
                    RoleId = 2
                },
                new User
                {
                    Id = 2,
                    FirstName = "Toster",
                    MiddleName = "Test",
                    LastName = "Tosterovich",
                    Email = "test@gmail.com",
                    Password = "test",
                    RoleId = 1,
                    UserRoomId = 1
                }
            );
        }

        private static void Seed(EntityTypeBuilder<Book> builder)
        {
            builder.HasData(
                new Book
                {
                    Id = 1,
                    UserId = 2,
                    DateAdded = DateTime.Now,
                    LanguageId = 1,
                    Name = "Adventures of Junior",
                    State = BookState.Available
                }
            );
        }

        private static void Seed(EntityTypeBuilder<BookGenre> builder)
        {
            builder.HasData(
                new BookGenre
                {
                    BookId = 1,
                    GenreId = 1
                }
            );
        }

        private static void Seed(EntityTypeBuilder<BookAuthor> builder)
        {
            builder.HasData(
                new BookAuthor
                {
                    BookId = 1,
                    AuthorId = 1
                }
            );
        }
    }
}
