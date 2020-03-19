using Microsoft.EntityFrameworkCore.Migrations;

namespace BookCrossingBackEnd.Migrations
{
    public partial class migrationv3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Request_User_owner_id",
                table: "Request");

            migrationBuilder.AddForeignKey(
                name: "FK_Request_User_owner_id",
                table: "Request",
                column: "owner_id",
                principalTable: "User",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Request_User_owner_id",
                table: "Request");

            migrationBuilder.AddForeignKey(
                name: "FK_Request_User_owner_id",
                table: "Request",
                column: "owner_id",
                principalTable: "User",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
