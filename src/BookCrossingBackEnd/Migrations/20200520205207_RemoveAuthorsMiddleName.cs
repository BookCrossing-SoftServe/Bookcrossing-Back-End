using Microsoft.EntityFrameworkCore.Migrations;

namespace BookCrossingBackEnd.Migrations
{
    public partial class RemoveAuthorsMiddleName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "middlename",
                table: "Author");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "middlename",
                table: "Author",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);
        }
    }
}
