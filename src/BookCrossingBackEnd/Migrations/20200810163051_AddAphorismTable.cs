using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BookCrossingBackEnd.Migrations
{
    public partial class AddAphorismTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Aphorism",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    phrase = table.Column<string>(nullable: true),
                    phraseAuthor = table.Column<string>(nullable: false),
                    IsCurrent = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aphorism", x => x.id);
                });

            migrationBuilder.InsertData(
                table: "Aphorism",
                columns: new[] { "id", "IsCurrent", "phrase", "phraseAuthor" },
                values: new object[,]
                {
                    { 1, false, "…Учітесь,читайте,і чужому научайтесь,й свого не цурайтесь.", "Taras Shevchenko" },
                    { 24, false, "Є злочини гірші, ніж спалювати книги. Наприклад - не читати їх.", "Р. Бербері" },
                    { 25, false, "I am a part of all I have read.", "John Kieran" },
                    { 26, false, "Людину можна пізнати по тих книгах, які вона читає", "С. Самолов" },
                    { 27, false, "The books that the world calls immoral are the books that show the world its own shame.", "Oscar Wilde" },
                    { 28, false, "У книгах ми жадібно читаємо про те, на що не звертаємо уваги в житті.", "Е. Кроткий" },
                    { 29, false, "Perhaps there is some sort of secret homing instinct in books that brings them to their perfect readers.", "Mary Ann Shaffer and Annie Barrows" },
                    { 30, false, "Три найсмачніші запахи? Запах гарячої кави, свіжої випічки і сторінок нової книги", "Н. Ясмінська" },
                    { 31, false, "Writing is so difficult that I often feel that writers, having had their hell on earth, will escape punishment in the hereafter.", "Jessamyn West" },
                    { 32, false, "Книги – морська глибина: Хто в них пірне аж до дна, той, хоч і труду мав досить дивнії перлини виносить.", "-	І. Франко" },
                    { 33, false, "It is books that are a key to the wide world; if you can't do anything else, read all that you can. ", "Jane Hamilton" },
                    { 34, false, "Руйнуються царства, а книги живуть!", "M. Грибачов" },
                    { 35, false, "I wrote my first novel because I wanted to read it. ", "Toni Morrison" },
                    { 36, false, "Книга - велика річ, поки людина вміє нею користуватися", "O. Блок" },
                    { 37, false, "Everything in the world exists in order to end up as a book. ", "Stéphane Mallarmé" },
                    { 38, false, "Читання може бути трояке: перше - читати і не розуміти; друге - читати і розуміти; третє - читати і розуміти навіть те, чого не написано. ", "-	Я. Княжнін" },
                    { 39, false, "He felt about books as doctors feel about medicines, or managers about plays -- cynical but hopeful.", "-	Dame Rose Macauley" },
                    { 40, false, "Моя відрада - уявний політ над книгами зі сторінки на сторінку", "М. Рубакін" },
                    { 23, false, "The mere brute pleasure of reading -- the sort of pleasure a cow must have in grazing.", "G. K. Chesterton" },
                    { 21, false, "Literature is my Utopia. Here I am not disenfranchised. No barrier of the senses shuts me out from the sweet, gracious discourses of my book friends. They talk to me without embarrassment or awkwardness.", "Helen Keller" },
                    { 22, false, "Кімната без книг – все одно, що людина без душі.", "О. Довженко" },
                    { 10, false, "Книги - це ріки, що наповнюють моря", "Я. Мудрий" },
                    { 4, false, "Читання – це звичка, до якої не звикають, а хворіють на неї", "Д. Лихачов" },
                    { 5, false, "Access to knowledge is the superb, the supreme act of truly great civilizations. Of all the institutions that purport to do this, free libraries stand virtually alone in accomplishing this mission", "Toni Morrison" },
                    { 6, false, "…Учітесь, читайте, і чужому научайтесь, й свого не цурайтесь.", "Т. Шевченко" },
                    { 7, false, "The man who does not read good books has no advantage over the man who cannot read them.", "Mark Twain" },
                    { 8, false, "Книга… залишається німою не тільки для того, хто не вміє читати, а й для того, хто… не уміє витягти з мертвої букви живої думки", "К. Ушинський" },
                    { 9, false, "The First Book: Go ahead, it won't bite. Well... maybe a little. More a nip, like. A tingle. It's pleasurable, really. You see, it keeps on opening. You may fall in. Sure, it's hard to get started; remember learning to use knife and fork? Dig in: you'll never reach bottom. It's not like it's the end of the world -- just the world as you think you know it.", "Rita Dove" },
                    { 20, false, "…Дивною і ненатуральною здається людина, яка існує без книги", "Т. Шевченко" },
                    { 3, false, "Strange thoughts brew in your heart when you spend too much time with old books", "Aravind Adiga" },
                    { 11, false, "Reading is to the mind what exercise is to the body", "Joseph Addison" },
                    { 13, false, "The greatest gift is a passion for reading. It is cheap, it consoles, it distracts, it excites, it gives you the knowledge of the world and experience of a wide kind. It is a moral illumination. ", "Elizabeth Hardwick" },
                    { 14, false, "Книги – люди в палітурках", "А. Макаренко" },
                    { 15, false, "The book is good which puts me in a working mind", "Ralph Waldo Emerson" },
                    { 16, false, "Є у мене товариші вірні – книжки добрії", "М. Грушевський" },
                    { 17, false, "Writing a book of poetry is like dropping a rose petal down the Grand Canyon and waiting for the echo", "Don Marquis" },
                    { 18, false, "Хто полюбить книгу, той далеко піде у своєму розвитку. Книга рятує душу від здерев’яніння", "Т. Шевченко" },
                    { 19, false, "From the moment I picked your book up until I laid it down I was convulsed with laughter. Some day I intend reading it.", "Groucho Marx" },
                    { 12, false, "Кожна книга – крадіжка у власного життя. Чим більше читаєш, тим менше вмієш і хочеш жити сам", "М. Цвєтаєва" },
                    { 2, false, "Без книг порожнє людське життя", "Д. Бідний" }
                });

            migrationBuilder.UpdateData(
                table: "Book",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "DateAdded", "State" },
                values: new object[] { new DateTime(2020, 8, 10, 19, 30, 50, 33, DateTimeKind.Local).AddTicks(9661), "Available" });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "id",
                keyValue: 1,
                column: "role_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "id",
                keyValue: 2,
                column: "role_id",
                value: 2);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Aphorism");

            migrationBuilder.UpdateData(
                table: "Book",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "DateAdded", "State" },
                values: new object[] { new DateTime(2020, 7, 22, 19, 1, 23, 901, DateTimeKind.Local).AddTicks(4298), "Available" });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "id",
                keyValue: 1,
                column: "role_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "id",
                keyValue: 2,
                column: "role_id",
                value: 2);
        }
    }
}
