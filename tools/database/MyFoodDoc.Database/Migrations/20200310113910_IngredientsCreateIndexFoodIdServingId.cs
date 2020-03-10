using Microsoft.EntityFrameworkCore.Migrations;

namespace MyFoodDoc.Database.Migrations
{
    public partial class IngredientsCreateIndexFoodIdServingId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_FoodId_ServingId",
                schema: "Food",
                table: "Ingredients",
                columns: new[] { "FoodId", "ServingId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Ingredients_FoodId_ServingId",
                schema: "Food",
                table: "Ingredients");
        }
    }
}
