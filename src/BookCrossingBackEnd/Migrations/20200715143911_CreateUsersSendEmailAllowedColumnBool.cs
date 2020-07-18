using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BookCrossingBackEnd.Migrations
{
    public partial class CreateUsersSendEmailAllowedColumnBool : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool?>(
                name: "email_allowed",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: 1);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "email_allowed",
                table: "User");  
        }
    }
}
