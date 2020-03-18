using Microsoft.EntityFrameworkCore.Migrations;

namespace BookCrossingBackEnd.Migrations
{
    public partial class migrationv2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Request_Book_book_id",
                table: "Request");

            migrationBuilder.AddForeignKey(
                name: "FK_Request_Book_book_id",
                table: "Request",
                column: "book_id",
                principalTable: "Book",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Request_Book_book_id",
                table: "Request");

            migrationBuilder.AddForeignKey(
                name: "FK_Request_Book_book_id",
                table: "Request",
                column: "book_id",
                principalTable: "Book",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
