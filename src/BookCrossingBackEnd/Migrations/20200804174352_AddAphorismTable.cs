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
                    phraseAuthor = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aphorism", x => x.id);
                });

            migrationBuilder.InsertData(
                table: "Aphorism",
                columns: new[] { "id", "phrase", "phraseAuthor" },
                values: new object[] { 1, "…Учітесь,читайте,і чужому научайтесь,й свого не цурайтесь.", "Taras Shevchenko" });

            migrationBuilder.UpdateData(
                table: "Book",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "DateAdded", "State" },
                values: new object[] { new DateTime(2020, 8, 4, 20, 43, 50, 847, DateTimeKind.Local).AddTicks(3051), "Available" });

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
