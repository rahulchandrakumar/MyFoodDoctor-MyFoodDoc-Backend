using Microsoft.EntityFrameworkCore.Migrations;

namespace MyFoodDoc.Database.Migrations
{
    public partial class MealsCreateIndexUserIdDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Meals_UserId_Date_Type",
                schema: "Diary",
                table: "Meals");

            migrationBuilder.CreateIndex(
                name: "IX_Meals_UserId_Date",
                schema: "Diary",
                table: "Meals",
                columns: new[] { "UserId", "Date" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Meals_UserId_Date",
                schema: "Diary",
                table: "Meals");

            migrationBuilder.CreateIndex(
                name: "IX_Meals_UserId_Date_Type",
                schema: "Diary",
                table: "Meals",
                columns: new[] { "UserId", "Date", "Type" },
                unique: true,
                filter: "Type IN ('Breakfast', 'Lunch', 'Dinner')");
        }
    }
}
