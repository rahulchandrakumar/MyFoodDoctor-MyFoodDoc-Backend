using Microsoft.EntityFrameworkCore.Migrations;

namespace MyFoodDoc.Database.Migrations
{
    public partial class IngredientsAddExternalNutritions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CaloriesExternal",
                schema: "Food",
                table: "Ingredients",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "CarbohydrateExternal",
                schema: "Food",
                table: "Ingredients",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "CholesterolExternal",
                schema: "Food",
                table: "Ingredients",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "FatExternal",
                schema: "Food",
                table: "Ingredients",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "FiberExternal",
                schema: "Food",
                table: "Ingredients",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MonounsaturatedFatExternal",
                schema: "Food",
                table: "Ingredients",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PolyunsaturatedFatExternal",
                schema: "Food",
                table: "Ingredients",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PotassiumExternal",
                schema: "Food",
                table: "Ingredients",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ProteinExternal",
                schema: "Food",
                table: "Ingredients",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SaturatedFatExternal",
                schema: "Food",
                table: "Ingredients",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SodiumExternal",
                schema: "Food",
                table: "Ingredients",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SugarExternal",
                schema: "Food",
                table: "Ingredients",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                schema: "System",
                table: "OptimizationAreas",
                keyColumn: "Id",
                keyValue: 1,
                column: "Text",
                value: @"Gemüse und Salat haben einen hohen Wasseranteil, wodurch sie sehr kalorienarm sind. Die enthaltenen Ballaststoffe quellen im Magen-Darm-Trakt auf, wodurch du lange satt bleibst.
Die Ballaststoffe aus Gemüse und Salat sorgen für eine gute Verdauung, wodurch dein Darm gesund bleibt.
Gemüse und Salat sind reich an Vitaminen und sekundären Pflanzenstoffen. Sie sorgen für ein starkes Immunsystem.");

            migrationBuilder.UpdateData(
                schema: "System",
                table: "OptimizationAreas",
                keyColumn: "Id",
                keyValue: 2,
                column: "Text",
                value: @"Zuckerreiche Lebensmittel liefern schnell, aber nur kurzfristige Energie. Es kommt zu einem sehr schnellen Anstieg des Blutzuckers, was zu einer sehr schnellen und starken Insulinausschüttung aus der Bauchspeicheldrüse führt.
Dieser Mechanismus fördert Heißhungerattacken und Übergewicht.");

            migrationBuilder.UpdateData(
                schema: "System",
                table: "OptimizationAreas",
                keyColumn: "Id",
                keyValue: 3,
                column: "Text",
                value: @"Eiweiß ist ein lebensnotweniger Nährstoff. Es sorgt für den Erhalt und den Aufbau unserer Muskulatur und unterstützt unser Immunsystem.
Zusätzlich sorgt eine eiweißreiche Mahlzeit für weniger Blutzuckerschwankungen und eine lang anhaltende Sättigung.");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CaloriesExternal",
                schema: "Food",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "CarbohydrateExternal",
                schema: "Food",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "CholesterolExternal",
                schema: "Food",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "FatExternal",
                schema: "Food",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "FiberExternal",
                schema: "Food",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "MonounsaturatedFatExternal",
                schema: "Food",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "PolyunsaturatedFatExternal",
                schema: "Food",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "PotassiumExternal",
                schema: "Food",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "ProteinExternal",
                schema: "Food",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "SaturatedFatExternal",
                schema: "Food",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "SodiumExternal",
                schema: "Food",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "SugarExternal",
                schema: "Food",
                table: "Ingredients");

            migrationBuilder.UpdateData(
                schema: "System",
                table: "OptimizationAreas",
                keyColumn: "Id",
                keyValue: 1,
                column: "Text",
                value: @"Gemüse und Salat haben einen hohen Wasseranteil, wodurch sie sehr kalorienarm sind. Die enthaltenen Ballaststoffe quellen im Magen-Darm-Trakt auf, wodurch du lange satt bleibst.
Die Ballaststoffe aus Gemüse und Salat sorgen für eine gute Verdauung, wodurch dein Darm gesund bleibt.
Gemüse und Salat sind reich an Vitaminen und sekundären Pflanzenstoffen. Sie sorgen für ein starkes Immunsystem.");

            migrationBuilder.UpdateData(
                schema: "System",
                table: "OptimizationAreas",
                keyColumn: "Id",
                keyValue: 2,
                column: "Text",
                value: @"Zuckerreiche Lebensmittel liefern schnell, aber nur kurzfristige Energie. Es kommt zu einem sehr schnellen Anstieg des Blutzuckers, was zu einer sehr schnellen und starken Insulinausschüttung aus der Bauchspeicheldrüse führt.
Dieser Mechanismus fördert Heißhungerattacken und Übergewicht.");

            migrationBuilder.UpdateData(
                schema: "System",
                table: "OptimizationAreas",
                keyColumn: "Id",
                keyValue: 3,
                column: "Text",
                value: @"Eiweiß ist ein lebensnotweniger Nährstoff. Es sorgt für den Erhalt und den Aufbau unserer Muskulatur und unterstützt unser Immunsystem.
Zusätzlich sorgt eine eiweißreiche Mahlzeit für weniger Blutzuckerschwankungen und eine lang anhaltende Sättigung.");
        }
    }
}
