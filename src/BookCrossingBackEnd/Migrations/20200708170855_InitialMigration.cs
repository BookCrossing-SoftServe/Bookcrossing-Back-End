using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BookCrossingBackEnd.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Author",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    firstname = table.Column<string>(maxLength: 100, nullable: false),
                    lastname = table.Column<string>(maxLength: 100, nullable: false),
                    is_confirmed = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Author", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Genre",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genre", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Language",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Language", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Location",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    city = table.Column<string>(maxLength: 30, nullable: false),
                    street = table.Column<string>(maxLength: 50, nullable: false),
                    office_name = table.Column<string>(maxLength: 10, nullable: true),
                    is_active = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Location", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ResetPassword",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    confirmation_number = table.Column<string>(nullable: false),
                    reset_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResetPassword", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ScheduleJob",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    scheduleId = table.Column<string>(maxLength: 50, nullable: false),
                    requestId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleJob", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "UserRoom",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    location_id = table.Column<int>(nullable: false),
                    room_number = table.Column<string>(maxLength: 7, nullable: true)
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

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    firstname = table.Column<string>(maxLength: 20, nullable: false),
                    middlename = table.Column<string>(maxLength: 20, nullable: true),
                    lastname = table.Column<string>(maxLength: 20, nullable: false),
                    email = table.Column<string>(maxLength: 40, nullable: false),
                    RefreshToken = table.Column<string>(nullable: true),
                    password = table.Column<string>(maxLength: 32, nullable: false),
                    role_id = table.Column<int>(nullable: false, defaultValue: 1),
                    user_room_id = table.Column<int>(nullable: true),
                    birth_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    registered_date = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.id);
                    table.ForeignKey(
                        name: "FK_User_Role_role_id",
                        column: x => x.role_id,
                        principalTable: "Role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_User_UserRoom_user_room_id",
                        column: x => x.user_room_id,
                        principalTable: "UserRoom",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Book",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(maxLength: 100, nullable: false),
                    user_id = table.Column<int>(nullable: false),
                    publisher = table.Column<string>(maxLength: 100, nullable: true),
                    State = table.Column<string>(maxLength: 50, nullable: true, defaultValue: "Available"),
                    rating = table.Column<double>(nullable: false, defaultValue: 0.0),
                    notice = table.Column<string>(maxLength: 255, nullable: true),
                    imagepath = table.Column<string>(maxLength: 260, nullable: true),
                    LanguageId = table.Column<int>(nullable: false),
                    DateAdded = table.Column<DateTime>(nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Book", x => x.id);
                    table.ForeignKey(
                        name: "FK_Book_Language_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Language",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Book_User_user_id",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefreshToken",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshToken_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookAuthor",
                columns: table => new
                {
                    book_id = table.Column<int>(nullable: false),
                    author_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookAuthor", x => new { x.book_id, x.author_id });
                    table.ForeignKey(
                        name: "FK_BookAuthor_Author_author_id",
                        column: x => x.author_id,
                        principalTable: "Author",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookAuthor_Book_book_id",
                        column: x => x.book_id,
                        principalTable: "Book",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookGenre",
                columns: table => new
                {
                    book_id = table.Column<int>(nullable: false),
                    genre_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookGenre", x => new { x.book_id, x.genre_id });
                    table.ForeignKey(
                        name: "FK_BookGenre_Book_book_id",
                        column: x => x.book_id,
                        principalTable: "Book",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookGenre_Genre_genre_id",
                        column: x => x.genre_id,
                        principalTable: "Genre",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Request",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    book_id = table.Column<int>(nullable: false),
                    owner_id = table.Column<int>(nullable: false),
                    user_id = table.Column<int>(nullable: false),
                    request_date = table.Column<DateTime>(type: "datetime", nullable: false),
                    receive_date = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Request", x => x.id);
                    table.ForeignKey(
                        name: "FK_Request_Book_book_id",
                        column: x => x.book_id,
                        principalTable: "Book",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Request_User_owner_id",
                        column: x => x.owner_id,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Request_User_user_id",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Author",
                columns: new[] { "id", "firstname", "is_confirmed", "lastname" },
                values: new object[] { 1, "Bernard", true, "Fernandez" });

            migrationBuilder.InsertData(
                table: "Genre",
                columns: new[] { "id", "name" },
                values: new object[] { 1, "Fantasy" });

            migrationBuilder.InsertData(
                table: "Language",
                columns: new[] { "id", "name" },
                values: new object[] { 1, "Ukrainian" });

            migrationBuilder.InsertData(
                table: "Location",
                columns: new[] { "id", "city", "is_active", "office_name", "street" },
                values: new object[] { 1, "Lviv", true, "SoftServe", "Gorodoc'kogo" });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 1, "User" },
                    { 2, "Admin" }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "id", "birth_date", "email", "firstname", "lastname", "middlename", "password", "RefreshToken", "role_id", "user_room_id" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@gmail.com", "Admin", "Adminovich", "Adminovski", "admin", null, 2, null });

            migrationBuilder.InsertData(
                table: "UserRoom",
                columns: new[] { "id", "location_id", "room_number" },
                values: new object[] { 1, 1, "4040" });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "id", "birth_date", "email", "firstname", "lastname", "middlename", "password", "RefreshToken", "role_id", "user_room_id" },
                values: new object[] { 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "test@gmail.com", "Toster", "Tosterovich", "Test", "test", null, 1, 1 });

            migrationBuilder.InsertData(
                table: "Book",
                columns: new[] { "id", "DateAdded", "imagepath", "LanguageId", "name", "notice", "publisher", "State", "user_id" },
                values: new object[] { 1, new DateTime(2020, 7, 8, 20, 8, 55, 242, DateTimeKind.Local).AddTicks(5168), null, 1, "Adventures of Junior", null, null, "Available", 2 });

            migrationBuilder.InsertData(
                table: "BookAuthor",
                columns: new[] { "book_id", "author_id" },
                values: new object[] { 1, 1 });

            migrationBuilder.InsertData(
                table: "BookGenre",
                columns: new[] { "book_id", "genre_id" },
                values: new object[] { 1, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_Book_LanguageId",
                table: "Book",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Book_user_id",
                table: "Book",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_BookAuthor_author_id",
                table: "BookAuthor",
                column: "author_id");

            migrationBuilder.CreateIndex(
                name: "IX_BookGenre_genre_id",
                table: "BookGenre",
                column: "genre_id");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_UserId",
                table: "RefreshToken",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Request_book_id",
                table: "Request",
                column: "book_id");

            migrationBuilder.CreateIndex(
                name: "IX_Request_owner_id",
                table: "Request",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "IX_Request_user_id",
                table: "Request",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_User_role_id",
                table: "User",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_User_user_room_id",
                table: "User",
                column: "user_room_id");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoom_location_id",
                table: "UserRoom",
                column: "location_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookAuthor");

            migrationBuilder.DropTable(
                name: "BookGenre");

            migrationBuilder.DropTable(
                name: "RefreshToken");

            migrationBuilder.DropTable(
                name: "Request");

            migrationBuilder.DropTable(
                name: "ResetPassword");

            migrationBuilder.DropTable(
                name: "ScheduleJob");

            migrationBuilder.DropTable(
                name: "Author");

            migrationBuilder.DropTable(
                name: "Genre");

            migrationBuilder.DropTable(
                name: "Book");

            migrationBuilder.DropTable(
                name: "Language");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "UserRoom");

            migrationBuilder.DropTable(
                name: "Location");
        }
    }
}
