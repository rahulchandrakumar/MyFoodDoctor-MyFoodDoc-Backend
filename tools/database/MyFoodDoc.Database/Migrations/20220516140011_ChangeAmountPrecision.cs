using Microsoft.EntityFrameworkCore.Migrations;
using MyFoodDoc.Application.Entities.Diary;

#nullable disable

namespace MyFoodDoc.Database.Migrations
{
    public partial class ChangeAmountPrecision : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                schema: "Diary",
                table: "MealsIngredients",
                type: "decimal(18,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                schema: "Diary",
                table: "FavouritesIngredients",
                type: "decimal(18,4)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
