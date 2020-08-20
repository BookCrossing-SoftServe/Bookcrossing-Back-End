using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BookCrossingBackEnd.Migrations
{
    public partial class SetMaxLength512ForNotificationMessage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "Notification",
                maxLength: 512,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Aphorism",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "phrase", "phraseAuthor" },
                values: new object[] { "A book is not only a friend, it makes friends for you. When you have possessed a book with mind and spirit, you are enriched. But when you pass it on you are enriched threefold.", "Henry Miller" });

            migrationBuilder.UpdateData(
                table: "Aphorism",
                keyColumn: "id",
                keyValue: 2,
                column: "phrase",
                value: "Без книг порожнє людське життя.");

            migrationBuilder.UpdateData(
                table: "Aphorism",
                keyColumn: "id",
                keyValue: 3,
                column: "phrase",
                value: "Strange thoughts brew in your heart when you spend too much time with old books.");

            migrationBuilder.UpdateData(
                table: "Aphorism",
                keyColumn: "id",
                keyValue: 4,
                column: "phrase",
                value: "Читання – це звичка, до якої не звикають, а хворіють на неї.");

            migrationBuilder.UpdateData(
                table: "Aphorism",
                keyColumn: "id",
                keyValue: 5,
                column: "phrase",
                value: "Access to knowledge is the superb, the supreme act of truly great civilizations. Of all the institutions that purport to do this, free libraries stand virtually alone in accomplishing this mission.");

            migrationBuilder.UpdateData(
                table: "Aphorism",
                keyColumn: "id",
                keyValue: 8,
                column: "phrase",
                value: "Книга… залишається німою не тільки для того, хто не вміє читати, а й для того, хто… не уміє витягти з мертвої букви живої думки.");

            migrationBuilder.UpdateData(
                table: "Aphorism",
                keyColumn: "id",
                keyValue: 9,
                column: "phrase",
                value: "The First Book: Go ahead, it won't bite. Well... maybe a little. More a nip, like. A tingle. It's pleasurable, really. You see, it keeps on opening. You may fall in. Sure, it's hard to get started; remember learning to use knife and fork? Dig in: you'll never reach bottom. It's not like it's the end of the world - just the world as you think you know it.");

            migrationBuilder.UpdateData(
                table: "Aphorism",
                keyColumn: "id",
                keyValue: 10,
                column: "phrase",
                value: "Книги - це ріки, що наповнюють моря.");

            migrationBuilder.UpdateData(
                table: "Aphorism",
                keyColumn: "id",
                keyValue: 11,
                column: "phrase",
                value: "Reading is to the mind what exercise is to the body.");

            migrationBuilder.UpdateData(
                table: "Aphorism",
                keyColumn: "id",
                keyValue: 12,
                column: "phrase",
                value: "Кожна книга – крадіжка у власного життя. Чим більше читаєш, тим менше вмієш і хочеш жити сам.");

            migrationBuilder.UpdateData(
                table: "Aphorism",
                keyColumn: "id",
                keyValue: 13,
                column: "phrase",
                value: "The greatest gift is a passion for reading. It is cheap, it consoles, it distracts, it excites, it gives you the knowledge of the world and experience of a wide kind. It is a moral illumination.");

            migrationBuilder.UpdateData(
                table: "Aphorism",
                keyColumn: "id",
                keyValue: 14,
                column: "phrase",
                value: "Книги – люди в палітурках.");

            migrationBuilder.UpdateData(
                table: "Aphorism",
                keyColumn: "id",
                keyValue: 15,
                column: "phrase",
                value: "The book is good which puts me in a working mind.");

            migrationBuilder.UpdateData(
                table: "Aphorism",
                keyColumn: "id",
                keyValue: 16,
                column: "phrase",
                value: "Є у мене товариші вірні – книжки добрії.");

            migrationBuilder.UpdateData(
                table: "Aphorism",
                keyColumn: "id",
                keyValue: 17,
                column: "phrase",
                value: "Writing a book of poetry is like dropping a rose petal down the Grand Canyon and waiting for the echo.");

            migrationBuilder.UpdateData(
                table: "Aphorism",
                keyColumn: "id",
                keyValue: 18,
                column: "phrase",
                value: "Хто полюбить книгу, той далеко піде у своєму розвитку. Книга рятує душу від здерев’яніння.");

            migrationBuilder.UpdateData(
                table: "Aphorism",
                keyColumn: "id",
                keyValue: 20,
                column: "phrase",
                value: "…Дивною і ненатуральною здається людина, яка існує без книги.");

            migrationBuilder.UpdateData(
                table: "Aphorism",
                keyColumn: "id",
                keyValue: 23,
                column: "phrase",
                value: "The mere brute pleasure of reading - the sort of pleasure a cow must have in grazing.");

            migrationBuilder.UpdateData(
                table: "Aphorism",
                keyColumn: "id",
                keyValue: 26,
                column: "phrase",
                value: "Людину можна пізнати по тих книгах, які вона читає.");

            migrationBuilder.UpdateData(
                table: "Aphorism",
                keyColumn: "id",
                keyValue: 30,
                column: "phrase",
                value: "Три найсмачніші запахи? Запах гарячої кави, свіжої випічки і сторінок нової книги.");

            migrationBuilder.UpdateData(
                table: "Aphorism",
                keyColumn: "id",
                keyValue: 33,
                column: "phrase",
                value: "It is books that are a key to the wide world; if you can't do anything else, read all that you can.");

            migrationBuilder.UpdateData(
                table: "Aphorism",
                keyColumn: "id",
                keyValue: 35,
                column: "phrase",
                value: "I wrote my first novel because I wanted to read it.");

            migrationBuilder.UpdateData(
                table: "Aphorism",
                keyColumn: "id",
                keyValue: 36,
                column: "phrase",
                value: "Книга - велика річ, поки людина вміє нею користуватися.");

            migrationBuilder.UpdateData(
                table: "Aphorism",
                keyColumn: "id",
                keyValue: 37,
                column: "phrase",
                value: "Everything in the world exists in order to end up as a book.");

            migrationBuilder.UpdateData(
                table: "Aphorism",
                keyColumn: "id",
                keyValue: 38,
                columns: new[] { "phrase", "phraseAuthor" },
                values: new object[] { "Читання може бути трояке: перше - читати і не розуміти; друге - читати і розуміти; третє - читати і розуміти навіть те, чого не написано.", "Я. Княжнін" });

            migrationBuilder.UpdateData(
                table: "Aphorism",
                keyColumn: "id",
                keyValue: 39,
                column: "phraseAuthor",
                value: "Dame Rose Macauley");

            migrationBuilder.UpdateData(
                table: "Aphorism",
                keyColumn: "id",
                keyValue: 40,
                column: "phrase",
                value: "Моя відрада - уявний політ над книгами зі сторінки на сторінку.");

            migrationBuilder.UpdateData(
                table: "Book",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "DateAdded", "State" },
                values: new object[] { new DateTime(2020, 8, 20, 21, 51, 8, 110, DateTimeKind.Local).AddTicks(8410), "Available" });

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
            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "Notification",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 512,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Aphorism",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "phrase", "phraseAuthor" },
                values: new object[] { "…Учітесь,читайте,і чужому научайтесь,й свого не цурайтесь.", "Taras Shevchenko" });

            migrationBuilder.UpdateData(
                table: "Aphorism",
                keyColumn: "id",
                keyValue: 2,
                column: "phrase",
                value: "Без книг порожнє людське життя");

            migrationBuilder.UpdateData(
                table: "Aphorism",
                keyColumn: "id",
                keyValue: 3,
                column: "phrase",
                value: "Strange thoughts brew in your heart when you spend too much time with old books");

            migrationBuilder.UpdateData(
                table: "Aphorism",
                keyColumn: "id",
                keyValue: 4,
                column: "phrase",
                value: "Читання – це звичка, до якої не звикають, а хворіють на неї");

            migrationBuilder.UpdateData(
                table: "Aphorism",
                keyColumn: "id",
                keyValue: 5,
                column: "phrase",
                value: "Access to knowledge is the superb, the supreme act of truly great civilizations. Of all the institutions that purport to do this, free libraries stand virtually alone in accomplishing this mission");

            migrationBuilder.UpdateData(
                table: "Aphorism",
                keyColumn: "id",
                keyValue: 8,
                column: "phrase",
                value: "Книга… залишається німою не тільки для того, хто не вміє читати, а й для того, хто… не уміє витягти з мертвої букви живої думки");

            migrationBuilder.UpdateData(
                table: "Aphorism",
                keyColumn: "id",
                keyValue: 9,
                column: "phrase",
                value: "The First Book: Go ahead, it won't bite. Well... maybe a little. More a nip, like. A tingle. It's pleasurable, really. You see, it keeps on opening. You may fall in. Sure, it's hard to get started; remember learning to use knife and fork? Dig in: you'll never reach bottom. It's not like it's the end of the world -- just the world as you think you know it.");

            migrationBuilder.UpdateData(
                table: "Aphorism",
                keyColumn: "id",
                keyValue: 10,
                column: "phrase",
                value: "Книги - це ріки, що наповнюють моря");

            migrationBuilder.UpdateData(
                table: "Aphorism",
                keyColumn: "id",
                keyValue: 11,
                column: "phrase",
                value: "Reading is to the mind what exercise is to the body");

            migrationBuilder.UpdateData(
                table: "Aphorism",
                keyColumn: "id",
                keyValue: 12,
                column: "phrase",
                value: "Кожна книга – крадіжка у власного життя. Чим більше читаєш, тим менше вмієш і хочеш жити сам");

            migrationBuilder.UpdateData(
                table: "Aphorism",
                keyColumn: "id",
                keyValue: 13,
                column: "phrase",
                value: "The greatest gift is a passion for reading. It is cheap, it consoles, it distracts, it excites, it gives you the knowledge of the world and experience of a wide kind. It is a moral illumination. ");

            migrationBuilder.UpdateData(
                table: "Aphorism",
                keyColumn: "id",
                keyValue: 14,
                column: "phrase",
                value: "Книги – люди в палітурках");

            migrationBuilder.UpdateData(
                table: "Aphorism",
                keyColumn: "id",
                keyValue: 15,
                column: "phrase",
                value: "The book is good which puts me in a working mind");

            migrationBuilder.UpdateData(
                table: "Aphorism",
                keyColumn: "id",
                keyValue: 16,
                column: "phrase",
                value: "Є у мене товариші вірні – книжки добрії");

            migrationBuilder.UpdateData(
                table: "Aphorism",
                keyColumn: "id",
                keyValue: 17,
                column: "phrase",
                value: "Writing a book of poetry is like dropping a rose petal down the Grand Canyon and waiting for the echo");

            migrationBuilder.UpdateData(
                table: "Aphorism",
                keyColumn: "id",
                keyValue: 18,
                column: "phrase",
                value: "Хто полюбить книгу, той далеко піде у своєму розвитку. Книга рятує душу від здерев’яніння");

            migrationBuilder.UpdateData(
                table: "Aphorism",
                keyColumn: "id",
                keyValue: 20,
                column: "phrase",
                value: "…Дивною і ненатуральною здається людина, яка існує без книги");

            migrationBuilder.UpdateData(
                table: "Aphorism",
                keyColumn: "id",
                keyValue: 23,
                column: "phrase",
                value: "The mere brute pleasure of reading -- the sort of pleasure a cow must have in grazing.");

            migrationBuilder.UpdateData(
                table: "Aphorism",
                keyColumn: "id",
                keyValue: 26,
                column: "phrase",
                value: "Людину можна пізнати по тих книгах, які вона читає");

            migrationBuilder.UpdateData(
                table: "Aphorism",
                keyColumn: "id",
                keyValue: 30,
                column: "phrase",
                value: "Три найсмачніші запахи? Запах гарячої кави, свіжої випічки і сторінок нової книги");

            migrationBuilder.UpdateData(
                table: "Aphorism",
                keyColumn: "id",
                keyValue: 33,
                column: "phrase",
                value: "It is books that are a key to the wide world; if you can't do anything else, read all that you can. ");

            migrationBuilder.UpdateData(
                table: "Aphorism",
                keyColumn: "id",
                keyValue: 35,
                column: "phrase",
                value: "I wrote my first novel because I wanted to read it. ");

            migrationBuilder.UpdateData(
                table: "Aphorism",
                keyColumn: "id",
                keyValue: 36,
                column: "phrase",
                value: "Книга - велика річ, поки людина вміє нею користуватися");

            migrationBuilder.UpdateData(
                table: "Aphorism",
                keyColumn: "id",
                keyValue: 37,
                column: "phrase",
                value: "Everything in the world exists in order to end up as a book. ");

            migrationBuilder.UpdateData(
                table: "Aphorism",
                keyColumn: "id",
                keyValue: 38,
                columns: new[] { "phrase", "phraseAuthor" },
                values: new object[] { "Читання може бути трояке: перше - читати і не розуміти; друге - читати і розуміти; третє - читати і розуміти навіть те, чого не написано. ", "-	Я. Княжнін" });

            migrationBuilder.UpdateData(
                table: "Aphorism",
                keyColumn: "id",
                keyValue: 39,
                column: "phraseAuthor",
                value: "-	Dame Rose Macauley");

            migrationBuilder.UpdateData(
                table: "Aphorism",
                keyColumn: "id",
                keyValue: 40,
                column: "phrase",
                value: "Моя відрада - уявний політ над книгами зі сторінки на сторінку");

            migrationBuilder.UpdateData(
                table: "Book",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "DateAdded", "State" },
                values: new object[] { new DateTime(2020, 8, 20, 13, 1, 43, 971, DateTimeKind.Local).AddTicks(9242), "Available" });

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
