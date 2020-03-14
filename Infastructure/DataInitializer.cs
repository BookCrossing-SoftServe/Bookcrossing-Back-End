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
            if (!context.Users.Any())
            {
                context.Users.Add(new User()
                {
                    FirstName = "Ivan",
                    MiddleName = "Ivanovych",
                    LastName = "Petrenko"

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
        }
    }
}
