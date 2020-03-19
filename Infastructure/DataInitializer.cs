using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Entities;
using Infrastructure;

namespace Infastructure
{
    public class DataInitializer
    {
        public static void Initialize(BookCrossingContext context)
        {
            if (!context.Role.Any())
            {
                context.Role.Add(new Role()
                {
                    Id = 1,
                    Name = "Admin"
                });
                context.SaveChanges();
            }
            if (!context.User.Any())
            {
                context.User.Add(new User()
                {
                    FirstName = "Roman",
                    MiddleName = "Ferents",
                    LastName = "Andriyovych",
                    Email = "ferencrman@gmail.com",
                    Password = "password",
                    RoleId = 1
                });
                context.SaveChanges();
            }
            if (!context.Author.Any())
            {
                context.Author.Add(new Author()
                {
                    FirstName = "List",
                    MiddleName = "Superman",
                    LastName = "Ferents"

                });
                context.SaveChanges();
            }
            if (!context.Genre.Any())
            {
                context.Genre.Add(new Genre()
                {
                    Name = "Fantazy"

                });
                context.SaveChanges();
            }
            if (!context.Book.Any())
            {
                context.Book.Add(new Book()
                {
                    UserId = 1,
                    Name = "CLR via C#",
                    Available = true,
                    Publisher = "Kolosok"

                });
                context.SaveChanges();
            }
            if (!context.Location.Any())
            {
                context.Location.Add(new Location()
                {
                    City = "Lviv",
                    Street = "Fedkovycha 13",
                    OfficeName = "4"

                });
                context.SaveChanges();
            }
            if (!context.BookAuthor.Any())
            {
                context.BookAuthor.Add(new BookAuthor()
                {
                    BookId = 1,
                    AuthorId = 1
                    
                });
                context.SaveChanges();
            }
            if (!context.BookGenre.Any())
            {
                context.BookGenre.Add(new BookGenre()
                {
                    BookId = 1,
                    GenreId = 1
                });
                context.SaveChanges();
            }
            if (!context.UserLocation.Any())
            {
                context.UserLocation.Add(new UserLocation()
                {
                    UserId = 1,
                    LocationId = 1,
                    RoomNumber = 4014
                });
                context.SaveChanges();
            }
        }
    }
}
