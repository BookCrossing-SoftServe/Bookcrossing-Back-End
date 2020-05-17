using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BookCrossingBackEnd.Migrations
{
    public partial class FixDatabaseLocations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Role_role_id",
                table: "User");

            migrationBuilder.DropTable(
                name: "UserLocation");

            migrationBuilder.AddColumn<DateTime>(
                name: "birth_date",
                table: "User",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "registered_date",
                table: "User",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<int>(
                name: "user_room_id",
                table: "User",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserRoom",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    location_id = table.Column<int>(nullable: false),
                    room_number = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoom", x => x.id);
                    table.ForeignKey(
                        name: "FK_UserRoom_Location_location_id",
                        column: x => x.location_id,
                        principalTable: "Location",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_user_room_id",
                table: "User",
                column: "user_room_id");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoom_location_id",
                table: "UserRoom",
                column: "location_id");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Role_role_id",
                table: "User",
                column: "role_id",
                principalTable: "Role",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_User_UserRoom_user_room_id",
                table: "User",
                column: "user_room_id",
                principalTable: "UserRoom",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Role_role_id",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "FK_User_UserRoom_user_room_id",
                table: "User");

            migrationBuilder.DropTable(
                name: "UserRoom");

            migrationBuilder.DropIndex(
                name: "IX_User_user_room_id",
                table: "User");

            migrationBuilder.DropColumn(
                name: "birth_date",
                table: "User");

            migrationBuilder.DropColumn(
                name: "registered_date",
                table: "User");

            migrationBuilder.DropColumn(
                name: "user_room_id",
                table: "User");

            migrationBuilder.CreateTable(
                name: "UserLocation",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false),
                    location_id = table.Column<int>(type: "int", nullable: false),
                    room_number = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLocation", x => new { x.user_id, x.location_id });
                    table.ForeignKey(
                        name: "FK_UserLocation_Location_location_id",
                        column: x => x.location_id,
                        principalTable: "Location",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserLocation_User_user_id",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserLocation_location_id",
                table: "UserLocation",
                column: "location_id");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Role_role_id",
                table: "User",
                column: "role_id",
                principalTable: "Role",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
