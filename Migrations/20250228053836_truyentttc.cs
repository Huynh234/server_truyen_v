using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace server_truyen_v.Migrations
{
    /// <inheritdoc />
    public partial class truyentttc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Story",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StoryCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StoryName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CoverImage = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    StoryAuthor = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    TypeStory = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    TypeDetailStory = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Story", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DetailStories",
                columns: table => new
                {
                    DetailID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StoryID = table.Column<int>(type: "int", nullable: false),
                    SttName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UrlImg = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: false),
                    Views = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetailStories", x => x.DetailID);
                    table.ForeignKey(
                        name: "FK_DetailStories_Story_StoryID",
                        column: x => x.StoryID,
                        principalTable: "Story",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comment",
                columns: table => new
                {
                    CommentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DetailID = table.Column<int>(type: "int", nullable: false),
                    CommentText = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Ratting = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comment", x => x.CommentID);
                    table.ForeignKey(
                        name: "FK_Comment_DetailStories_DetailID",
                        column: x => x.DetailID,
                        principalTable: "DetailStories",
                        principalColumn: "DetailID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comment_DetailID",
                table: "Comment",
                column: "DetailID");

            migrationBuilder.CreateIndex(
                name: "IX_DetailStories_StoryID",
                table: "DetailStories",
                column: "StoryID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comment");

            migrationBuilder.DropTable(
                name: "DetailStories");

            migrationBuilder.DropTable(
                name: "Story");
        }
    }
}
