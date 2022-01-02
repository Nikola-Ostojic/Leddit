using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Backend.DAL.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    ImageUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    UserName = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Role = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    Token = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: true),
                    ValidFor = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Movies",
                columns: new[] { "Id", "Created", "ImageUrl", "Modified", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2020, 9, 20, 17, 25, 18, 942, DateTimeKind.Local).AddTicks(2408), "https://gortoncenter.org/wp-content/uploads/2018/05/The-Martian.jpg", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "The Martian" },
                    { 2, new DateTime(2020, 9, 20, 17, 25, 18, 942, DateTimeKind.Local).AddTicks(2461), "https://m.media-amazon.com/images/M/MV5BMjQ3OTgzMzY4NF5BMl5BanBnXkFtZTgwOTc4OTQyMzI@._V1_.jpg", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kingsman: The Golden Circle" },
                    { 3, new DateTime(2020, 9, 20, 17, 25, 18, 942, DateTimeKind.Local).AddTicks(2465), "https://www.denofgeek.com/wp-content/uploads/2019/09/battlestar-galactica-reboot-1.jpeg", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Battlestar Galactica Razor" },
                    { 4, new DateTime(2020, 9, 20, 17, 25, 18, 942, DateTimeKind.Local).AddTicks(2469), "https://www.un.org/sites/un2.un.org/files/styles/large-article-image-style-16-9/public/field/image/dictator_quad-1024x768.jpg", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "The Dictator" },
                    { 5, new DateTime(2020, 9, 20, 17, 25, 18, 942, DateTimeKind.Local).AddTicks(2472), "https://i.pinimg.com/originals/a8/63/be/a863beaf54137860699352f35e6c5052.jpg", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Iron Man" },
                    { 6, new DateTime(2020, 9, 20, 17, 25, 18, 942, DateTimeKind.Local).AddTicks(2475), "https://icdn2.digitaltrends.com/image/digitaltrends/guardians-of-the-galaxy-vol-2-2.jpg", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Guardians of the Galaxy" },
                    { 7, new DateTime(2020, 9, 20, 17, 25, 18, 942, DateTimeKind.Local).AddTicks(2478), "https://m.media-amazon.com/images/M/MV5BMjA5NDQyMjc2NF5BMl5BanBnXkFtZTcwMjg5ODcyMw@@._V1_.jpg", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "How to train your dragon" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Created", "Email", "Modified", "Password", "Role", "UserName" },
                values: new object[,]
                {
                    { 1, new DateTime(2020, 9, 20, 17, 25, 18, 937, DateTimeKind.Local).AddTicks(6929), "user@levi9.com", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "b512d97e7cbf97c273e4db073bbb547aa65a84589227f8f3d9e4a72b9372a24d", 0, "User" },
                    { 2, new DateTime(2020, 9, 20, 17, 25, 18, 940, DateTimeKind.Local).AddTicks(7195), "admin@levi9.com", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "c1c224b03cd9bc7b6a86d77f5dace40191766c485cd55dc48caf9ac873335d6f", 1, "Admin" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Movies_Name",
                table: "Movies",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Movies");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
