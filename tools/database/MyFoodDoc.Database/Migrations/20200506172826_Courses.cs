using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyFoodDoc.Database.Migrations
{
    public partial class Courses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Course");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                schema: "System",
                table: "UserTargets",
                maxLength: 450,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Courses",
                schema: "Course",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    Title = table.Column<string>(maxLength: 1000, nullable: false),
                    Text = table.Column<string>(maxLength: 1000, nullable: false),
                    Order = table.Column<int>(nullable: false),
                    ImageId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Courses_Images_ImageId",
                        column: x => x.ImageId,
                        principalSchema: "System",
                        principalTable: "Images",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Chapters",
                schema: "Course",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true),
                    Title = table.Column<string>(maxLength: 1000, nullable: false),
                    Text = table.Column<string>(maxLength: 1000, nullable: false),
                    Order = table.Column<int>(nullable: false),
                    ImageId = table.Column<int>(nullable: true),
                    QuestionTitle = table.Column<string>(maxLength: 1000, nullable: false),
                    QuestionText = table.Column<string>(maxLength: 1000, nullable: false),
                    Answer = table.Column<bool>(nullable: false),
                    CourseId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chapters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Chapters_Courses_CourseId",
                        column: x => x.CourseId,
                        principalSchema: "Course",
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Chapters_Images_ImageId",
                        column: x => x.ImageId,
                        principalSchema: "System",
                        principalTable: "Images",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Subchapters",
                schema: "Course",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true),
                    Title = table.Column<string>(maxLength: 1000, nullable: false),
                    Text = table.Column<string>(maxLength: 1000, nullable: false),
                    Order = table.Column<int>(nullable: false),
                    ChapterId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subchapters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subchapters_Chapters_ChapterId",
                        column: x => x.ChapterId,
                        principalSchema: "Course",
                        principalTable: "Chapters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserAnswers",
                schema: "Course",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true),
                    UserId = table.Column<string>(maxLength: 450, nullable: false),
                    ChapterId = table.Column<int>(nullable: false),
                    Answer = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAnswers_Chapters_ChapterId",
                        column: x => x.ChapterId,
                        principalSchema: "Course",
                        principalTable: "Chapters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserAnswers_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "User",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Chapters_CourseId",
                schema: "Course",
                table: "Chapters",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Chapters_ImageId",
                schema: "Course",
                table: "Chapters",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_ImageId",
                schema: "Course",
                table: "Courses",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Subchapters_ChapterId",
                schema: "Course",
                table: "Subchapters",
                column: "ChapterId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAnswers_ChapterId",
                schema: "Course",
                table: "UserAnswers",
                column: "ChapterId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAnswers_UserId",
                schema: "Course",
                table: "UserAnswers",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Subchapters",
                schema: "Course");

            migrationBuilder.DropTable(
                name: "UserAnswers",
                schema: "Course");

            migrationBuilder.DropTable(
                name: "Chapters",
                schema: "Course");

            migrationBuilder.DropTable(
                name: "Courses",
                schema: "Course");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                schema: "System",
                table: "UserTargets",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 450);
        }
    }
}
