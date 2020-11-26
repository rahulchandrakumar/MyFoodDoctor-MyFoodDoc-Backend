using Microsoft.EntityFrameworkCore.Migrations;

namespace MyFoodDoc.Database.Migrations
{
    public partial class DiaryIndices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Liquids_UserId",
                schema: "Diary",
                table: "Liquids");

            migrationBuilder.DropIndex(
                name: "IX_Exercises_UserId",
                schema: "Diary",
                table: "Exercises");

            migrationBuilder.CreateIndex(
                name: "IX_Liquids_UserId_Date",
                schema: "Diary",
                table: "Liquids",
                columns: new[] { "UserId", "Date" });

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_UserId_Date",
                schema: "Diary",
                table: "Exercises",
                columns: new[] { "UserId", "Date" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Liquids_UserId_Date",
                schema: "Diary",
                table: "Liquids");

            migrationBuilder.DropIndex(
                name: "IX_Exercises_UserId_Date",
                schema: "Diary",
                table: "Exercises");

            migrationBuilder.CreateIndex(
                name: "IX_Liquids_UserId",
                schema: "Diary",
                table: "Liquids",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_UserId",
                schema: "Diary",
                table: "Exercises",
                column: "UserId");
        }
    }
}
