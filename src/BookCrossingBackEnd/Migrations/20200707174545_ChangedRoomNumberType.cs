using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BookCrossingBackEnd.Migrations
{
    public partial class ChangedRoomNumberType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "room_number",
                table: "UserRoom",
                maxLength: 7,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "UserRoom",
                keyColumn: "id",
                keyValue: 1,
                column: "room_number",
                value: "4040");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "room_number",
                table: "UserRoom",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 7,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "UserRoom",
                keyColumn: "id",
                keyValue: 1,
                column: "room_number",
                value: 4040);
        }
    }
}
