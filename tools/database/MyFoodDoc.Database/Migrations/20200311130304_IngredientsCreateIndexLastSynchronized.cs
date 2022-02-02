using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace MyFoodDoc.Database.Migrations
{
    public partial class IngredientsCreateIndexLastSynchronized : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "LastSynchronized",
                schema: "Food",
                table: "Ingredients",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "Date");

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_LastSynchronized",
                schema: "Food",
                table: "Ingredients",
                column: "LastSynchronized");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Ingredients_LastSynchronized",
                schema: "Food",
                table: "Ingredients");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastSynchronized",
                schema: "Food",
                table: "Ingredients",
                type: "Date",
                nullable: false,
                oldClrType: typeof(DateTime));
        }
    }
}
