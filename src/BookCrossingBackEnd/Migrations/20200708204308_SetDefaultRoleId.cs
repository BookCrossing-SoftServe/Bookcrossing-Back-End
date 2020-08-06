using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BookCrossingBackEnd.Migrations
{
    public partial class SetDefaultRoleId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "role_id",
                table: "User",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: null);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                 name: "role_id",
                 table: "User",
                 nullable: false,
                 defaultValue: null,
                 oldClrType: typeof(int),
                 oldType: "int",
                 oldDefaultValue: 1);
        }
    }
}
