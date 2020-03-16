using Microsoft.EntityFrameworkCore.Migrations;

namespace BookCrossingBackEnd.Migrations
{
    public partial class migrationv4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Request_Book_book_id",
                table: "Request");

            migrationBuilder.DropForeignKey(
                name: "FK_Request_User_owner_id",
                table: "Request");

            migrationBuilder.AddForeignKey(
                name: "FK_Request_Book",
                table: "Request",
                column: "book_id",
                principalTable: "Book",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Request_User_Owner",
                table: "Request",
                column: "owner_id",
                principalTable: "User",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Request_Book",
                table: "Request");

            migrationBuilder.DropForeignKey(
                name: "FK_Request_User_Owner",
                table: "Request");

            migrationBuilder.AddForeignKey(
                name: "FK_Request_Book_book_id",
                table: "Request",
                column: "book_id",
                principalTable: "Book",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Request_User_owner_id",
                table: "Request",
                column: "owner_id",
                principalTable: "User",
                principalColumn: "id");
        }
    }
}
