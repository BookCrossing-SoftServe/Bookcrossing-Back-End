using Microsoft.EntityFrameworkCore.Migrations;

namespace BookCrossingBackEnd.Migrations
{
    public partial class AddStateToBookTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "available",
                table: "Book");

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Book",
                maxLength: 50,
                nullable: true,
                defaultValue: "Available");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "State",
                table: "Book");

            migrationBuilder.AddColumn<bool>(
                name: "available",
                table: "Book",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
