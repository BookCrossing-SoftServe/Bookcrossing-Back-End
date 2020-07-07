using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BookCrossingBackEnd.Migrations
{
    public partial class Addresetpassword : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ResetPassword",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    confirmation_number = table.Column<string>(nullable: false),
                    reset_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_ResetPassword", x => x.id); });

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "ResetPassword");
        }
    }
}
