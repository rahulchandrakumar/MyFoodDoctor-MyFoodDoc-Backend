using Microsoft.EntityFrameworkCore.Migrations;

namespace MyFoodDoc.Database.Migrations
{
    public partial class ChapterAnswerText : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AnswerText1",
                schema: "Course",
                table: "Chapters",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AnswerText2",
                schema: "Course",
                table: "Chapters",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnswerText1",
                schema: "Course",
                table: "Chapters");

            migrationBuilder.DropColumn(
                name: "AnswerText2",
                schema: "Course",
                table: "Chapters");
        }
    }
}
