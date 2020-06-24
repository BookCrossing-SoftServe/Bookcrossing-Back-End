using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BookCrossingBackEnd.Migrations
{
    public partial class AddedSeeder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Book_Languages_LanguageId",
                table: "Book");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Languages",
                table: "Languages");

            migrationBuilder.RenameTable(
                name: "Languages",
                newName: "Language");

            migrationBuilder.AlterColumn<int>(
                name: "LanguageId",
                table: "Book",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Language",
                table: "Language",
                column: "Id");

            migrationBuilder.InsertData(
                table: "Author",
                columns: new[] { "id", "firstname", "is_confirmed", "lastname" },
                values: new object[] { 1, "Bernard", true, "Fernandez" });

            migrationBuilder.InsertData(
                table: "Genre",
                columns: new[] { "id", "name" },
                values: new object[] { 1, "Fantasy" });

            migrationBuilder.InsertData(
                table: "Language",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Ukrainian" });

            migrationBuilder.InsertData(
                table: "Location",
                columns: new[] { "id", "city", "is_active", "office_name", "street" },
                values: new object[] { 1, "Lviv", true, "SoftServe", "Gorodoc'kogo" });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 1, "User" },
                    { 2, "Admin" }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "id", "birth_date", "email", "firstname", "lastname", "middlename", "password", "RefreshToken", "role_id", "user_room_id" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@gmail.com", "Admin", "Adminovich", "Adminovski", "admin", null, 2, null });

            migrationBuilder.InsertData(
                table: "UserRoom",
                columns: new[] { "id", "location_id", "room_number" },
                values: new object[] { 1, 1, 4040 });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "id", "birth_date", "email", "firstname", "lastname", "middlename", "password", "RefreshToken", "role_id", "user_room_id" },
                values: new object[] { 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "test@gmail.com", "Toster", "Tosterovich", "Test", "test", null, 1, 1 });

            migrationBuilder.InsertData(
                table: "Book",
                columns: new[] { "id", "DateAdded", "imagepath", "LanguageId", "name", "notice", "publisher", "State", "user_id" },
                values: new object[] { 1, new DateTime(2020, 6, 25, 1, 51, 1, 329, DateTimeKind.Local).AddTicks(5666), null, 1, "Adventures of Junior", null, null, "Available", 2 });

            migrationBuilder.InsertData(
                table: "BookAuthor",
                columns: new[] { "book_id", "author_id" },
                values: new object[] { 1, 1 });

            migrationBuilder.InsertData(
                table: "BookGenre",
                columns: new[] { "book_id", "genre_id" },
                values: new object[] { 1, 1 });

            migrationBuilder.AddForeignKey(
                name: "FK_Book_Language_LanguageId",
                table: "Book",
                column: "LanguageId",
                principalTable: "Language",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Book_Language_LanguageId",
                table: "Book");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Language",
                table: "Language");

            migrationBuilder.DeleteData(
                table: "BookAuthor",
                keyColumns: new[] { "book_id", "author_id" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "BookGenre",
                keyColumns: new[] { "book_id", "genre_id" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Author",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Book",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Genre",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Language",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "UserRoom",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Location",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.RenameTable(
                name: "Language",
                newName: "Languages");

            migrationBuilder.AlterColumn<int>(
                name: "LanguageId",
                table: "Book",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Languages",
                table: "Languages",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Book_Languages_LanguageId",
                table: "Book",
                column: "LanguageId",
                principalTable: "Languages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
