using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BookCrossingBackEnd.Migrations
{
    public partial class AddedWishTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "email_allowed",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bool",
                oldDefaultValue: true);

            migrationBuilder.CreateTable(
                name: "Wish",
                columns: table => new
                {
                    user_id = table.Column<int>(nullable: false),
                    book_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wish", x => new { x.user_id, x.book_id });
                    table.ForeignKey(
                        name: "FK_Wish_Book_book_id",
                        column: x => x.book_id,
                        principalTable: "Book",
                        principalColumn: "id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Wish_User_user_id",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Wish_book_id",
                table: "Wish",
                column: "book_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Wish");

            migrationBuilder.AlterColumn<bool>(
                name: "email_allowed",
                table: "User",
                type: "bool",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);
        }
    }
}
