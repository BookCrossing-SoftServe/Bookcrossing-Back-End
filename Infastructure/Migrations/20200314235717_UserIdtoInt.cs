using Microsoft.EntityFrameworkCore.Migrations;

namespace Infastructure.Migrations
{
    public partial class UserIdtoInt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Book_User_user_id",
                table: "Book");

            migrationBuilder.DropForeignKey(
                name: "FK_Request_User_owner_id",
                table: "Request");

            migrationBuilder.AlterColumn<int>(
                name: "user_id",
                table: "UserLocation",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "User",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "user_id",
                table: "Request",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "owner_id",
                table: "Request",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "user_id",
                table: "Book",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Book_User_user_id",
                table: "Book",
                column: "user_id",
                principalTable: "User",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Request_User_owner_id",
                table: "Request",
                column: "owner_id",
                principalTable: "User",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Book_User_user_id",
                table: "Book");

            migrationBuilder.DropForeignKey(
                name: "FK_Request_User_owner_id",
                table: "Request");

            migrationBuilder.AlterColumn<string>(
                name: "user_id",
                table: "UserLocation",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "id",
                table: "User",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "user_id",
                table: "Request",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "owner_id",
                table: "Request",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "user_id",
                table: "Book",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Book_User_user_id",
                table: "Book",
                column: "user_id",
                principalTable: "User",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Request_User_owner_id",
                table: "Request",
                column: "owner_id",
                principalTable: "User",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
