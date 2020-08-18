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
            Seed(builder.Entity<Aphorism>());
        }

        private static void Seed(EntityTypeBuilder<Aphorism> builder)
        {
            builder.HasData(
                new Aphorism
                {
                    Id = 1,
                    Phrase = "…Учітесь,читайте,і чужому научайтесь,й свого не цурайтесь.",
                    PhraseAuthor = "Taras Shevchenko",
                },
                new Aphorism
                {
                    Id = 2,
                    Phrase = "Без книг порожнє людське життя.",
                    PhraseAuthor = "Д. Бідний",
                },
                new Aphorism
                {
                    Id = 3,
                    Phrase = "Strange thoughts brew in your heart when you spend too much time with old books.",
                    PhraseAuthor = "Aravind Adiga",
                },
                new Aphorism
                {
                    Id = 4,
                    Phrase = "Читання – це звичка, до якої не звикають, а хворіють на неї.",
                    PhraseAuthor = "Д. Лихачов",
                },
                new Aphorism
                {
                    Id = 5,
                    Phrase = "Access to knowledge is the superb, the supreme act of truly great civilizations. Of all the institutions that purport to do this, free libraries stand virtually alone in accomplishing this mission.",
                    PhraseAuthor = "Toni Morrison",
                },
                new Aphorism
                {
                    Id = 6,
                    Phrase = "…Учітесь, читайте, і чужому научайтесь, й свого не цурайтесь.",
                    PhraseAuthor = "Т. Шевченко",
                },
                new Aphorism
                {
                    Id = 7,
                    Phrase = "The man who does not read good books has no advantage over the man who cannot read them.",
                    PhraseAuthor = "Mark Twain",
                },
                new Aphorism
                {
                    Id = 8,
                    Phrase = "Книга… залишається німою не тільки для того, хто не вміє читати, а й для того, хто… не уміє витягти з мертвої букви живої думки.",
                    PhraseAuthor = "К. Ушинський",
                },
                new Aphorism
                {
                    Id = 9,
                    Phrase = "The First Book: Go ahead, it won't bite. Well... maybe a little. More a nip, like. A tingle. It's pleasurable, really. You see, it keeps on opening. You may fall in. Sure, it's hard to get started; remember learning to use knife and fork? Dig in: you'll never reach bottom. It's not like it's the end of the world - just the world as you think you know it.",
                    PhraseAuthor = "Rita Dove",
                },
                new Aphorism
                {
                    Id = 10,
                    Phrase = "Книги - це ріки, що наповнюють моря.",
                    PhraseAuthor = "Я. Мудрий",
                },
                new Aphorism
                {
                    Id = 11,
                    Phrase = "Reading is to the mind what exercise is to the body.",
                    PhraseAuthor = "Joseph Addison",
                },
                new Aphorism
                {
                    Id = 12,
                    Phrase = "Кожна книга – крадіжка у власного життя. Чим більше читаєш, тим менше вмієш і хочеш жити сам.",
                    PhraseAuthor = "М. Цвєтаєва",
                },
                new Aphorism
                {
                    Id = 13,
                    Phrase = "The greatest gift is a passion for reading. It is cheap, it consoles, it distracts, it excites, it gives you the knowledge of the world and experience of a wide kind. It is a moral illumination.",
                    PhraseAuthor = "Elizabeth Hardwick",
                },
                new Aphorism
                {
                    Id = 14,
                    Phrase = "Книги – люди в палітурках.",
                    PhraseAuthor = "А. Макаренко",
                },
                new Aphorism
                {
                    Id = 15,
                    Phrase = "The book is good which puts me in a working mind.",
                    PhraseAuthor = "Ralph Waldo Emerson",
                },
                new Aphorism
                {
                    Id = 16,
                    Phrase = "Є у мене товариші вірні – книжки добрії.",
                    PhraseAuthor = "М. Грушевський",
                },
                new Aphorism
                {
                    Id = 17,
                    Phrase = "Writing a book of poetry is like dropping a rose petal down the Grand Canyon and waiting for the echo.",
                    PhraseAuthor = "Don Marquis",
                },
                new Aphorism
                {
                    Id = 18,
                    Phrase = "Хто полюбить книгу, той далеко піде у своєму розвитку. Книга рятує душу від здерев’яніння.",
                    PhraseAuthor = "Т. Шевченко",
                },
                new Aphorism
                {
                    Id = 19,
                    Phrase = "From the moment I picked your book up until I laid it down I was convulsed with laughter. Some day I intend reading it.",
                    PhraseAuthor = "Groucho Marx",
                },
                new Aphorism
                {
                    Id = 20,
                    Phrase = "…Дивною і ненатуральною здається людина, яка існує без книги.",
                    PhraseAuthor = "Т. Шевченко",
                },
                new Aphorism
                {
                    Id = 21,
                    Phrase = "Literature is my Utopia. Here I am not disenfranchised. No barrier of the senses shuts me out from the sweet, gracious discourses of my book friends. They talk to me without embarrassment or awkwardness.",
                    PhraseAuthor = "Helen Keller",
                },
                new Aphorism
                {
                    Id = 22,
                    Phrase = "Кімната без книг – все одно, що людина без душі.",
                    PhraseAuthor = "О. Довженко",
                },
                new Aphorism
                {
                    Id = 23,
                    Phrase = "The mere brute pleasure of reading - the sort of pleasure a cow must have in grazing.",
                    PhraseAuthor = "G. K. Chesterton",
                },
                new Aphorism
                {
                    Id = 24,
                    Phrase = "Є злочини гірші, ніж спалювати книги. Наприклад - не читати їх.",
                    PhraseAuthor = "Р. Бербері",
                },
                new Aphorism
                {
                    Id = 25,
                    Phrase = "I am a part of all I have read.",
                    PhraseAuthor = "John Kieran",
                },
                new Aphorism
                {
                    Id = 26,
                    Phrase = "Людину можна пізнати по тих книгах, які вона читає.",
                    PhraseAuthor = "С. Самолов",
                },
                new Aphorism
                {
                    Id = 27,
                    Phrase = "The books that the world calls immoral are the books that show the world its own shame.",
                    PhraseAuthor = "Oscar Wilde",
                },
                new Aphorism
                {
                    Id = 28,
                    Phrase = "У книгах ми жадібно читаємо про те, на що не звертаємо уваги в житті.",
                    PhraseAuthor = "Е. Кроткий",
                },
                new Aphorism
                {
                    Id = 29,
                    Phrase = "Perhaps there is some sort of secret homing instinct in books that brings them to their perfect readers.",
                    PhraseAuthor = "Mary Ann Shaffer and Annie Barrows",
                },
                new Aphorism
                {
                    Id = 30,
                    Phrase = "Три найсмачніші запахи? Запах гарячої кави, свіжої випічки і сторінок нової книги.",
                    PhraseAuthor = "Н. Ясмінська",
                },
                new Aphorism
                {
                    Id = 31,
                    Phrase = "Writing is so difficult that I often feel that writers, having had their hell on earth, will escape punishment in the hereafter.",
                    PhraseAuthor = "Jessamyn West",
                },
                new Aphorism
                {
                    Id = 32,
                    Phrase = "Книги – морська глибина: Хто в них пірне аж до дна, той, хоч і труду мав досить дивнії перлини виносить.",
                    PhraseAuthor = "-	І. Франко",
                },
                new Aphorism
                {
                    Id = 33,
                    Phrase = "It is books that are a key to the wide world; if you can't do anything else, read all that you can.",
                    PhraseAuthor = "Jane Hamilton",
                },
                new Aphorism
                {
                    Id = 34,
                    Phrase = "Руйнуються царства, а книги живуть!",
                    PhraseAuthor = "M. Грибачов",
                },
                new Aphorism
                {
                    Id = 35,
                    Phrase = "I wrote my first novel because I wanted to read it.",
                    PhraseAuthor = "Toni Morrison",
                },
                new Aphorism
                {
                    Id = 36,
                    Phrase = "Книга - велика річ, поки людина вміє нею користуватися.",
                    PhraseAuthor = "O. Блок",
                },
                new Aphorism
                {
                    Id = 37,
                    Phrase = "Everything in the world exists in order to end up as a book.",
                    PhraseAuthor = "Stéphane Mallarmé",
                },
                new Aphorism
                {
                    Id = 38,
                    Phrase = "Читання може бути трояке: перше - читати і не розуміти; друге - читати і розуміти; третє - читати і розуміти навіть те, чого не написано.",
                    PhraseAuthor = "Я. Княжнін",
                },
                new Aphorism
                {
                    Id = 39,
                    Phrase = "He felt about books as doctors feel about medicines, or managers about plays -- cynical but hopeful.",
                    PhraseAuthor = "Dame Rose Macauley",
                },
                new Aphorism
                {
                    Id = 40,
                    Phrase = "Моя відрада - уявний політ над книгами зі сторінки на сторінку.",
                    PhraseAuthor = "М. Рубакін",
                }
            );
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
                    Id = 2,
                    FirstName = "Admin",
                    MiddleName = "Adminovski",
                    LastName = "Adminovich",
                    Email = "admin@gmail.com",
                    Password = "admin",
                    RoleId = 2
                },
                new User
                {
                    Id = 1,
                    FirstName = "Tester",
                    MiddleName = "Test",
                    LastName = "Testerovich",
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
