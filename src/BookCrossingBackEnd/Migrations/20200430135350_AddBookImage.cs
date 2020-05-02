using Microsoft.EntityFrameworkCore.Migrations;

namespace BookCrossingBackEnd.Migrations
{
    public partial class AddBookImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "imagepath",
                table: "Book",
                maxLength: 260,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "notice",
                table: "Book",
                maxLength: 500,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "imagepath",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "notice",
                table: "Book");
        }
    }
}
