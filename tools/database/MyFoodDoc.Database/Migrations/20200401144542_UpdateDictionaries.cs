using Microsoft.EntityFrameworkCore.Migrations;

namespace MyFoodDoc.Database.Migrations
{
    public partial class UpdateDictionaries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "System",
                table: "Diets",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Key", "Name" },
                values: new object[] { "mixed_diet", "Mischkost" });

            migrationBuilder.UpdateData(
                schema: "System",
                table: "Diets",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Key", "Name" },
                values: new object[] { "vegan", "Vegan" });

            migrationBuilder.UpdateData(
                schema: "System",
                table: "Diets",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Key", "Name" },
                values: new object[] { "less_carbohydrates", "Wenig Kohlenhydrate" });

            migrationBuilder.UpdateData(
                schema: "System",
                table: "Diets",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Key", "Name" },
                values: new object[] { "lactose_low", "Laktosearm" });

            migrationBuilder.UpdateData(
                schema: "System",
                table: "Diets",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Key", "Name" },
                values: new object[] { "fructose_low", "Fruktosearm" });

            migrationBuilder.UpdateData(
                schema: "System",
                table: "Diets",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Key", "Name" },
                values: new object[] { "gluten_free", "Glutenfrei" });

            migrationBuilder.UpdateData(
                schema: "System",
                table: "Motivations",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Anti Aging");

            migrationBuilder.UpdateData(
                schema: "System",
                table: "Motivations",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Gesünder leben");

            migrationBuilder.UpdateData(
                schema: "System",
                table: "Motivations",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Abnehmen");

            migrationBuilder.UpdateData(
                schema: "System",
                table: "Motivations",
                keyColumn: "Id",
                keyValue: 4,
                column: "Name",
                value: "Besser fühlen");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "System",
                table: "Diets",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Key", "Name" },
                values: new object[] { "vegan", "Vegan" });

            migrationBuilder.UpdateData(
                schema: "System",
                table: "Diets",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Key", "Name" },
                values: new object[] { "interval_fasting", "Intervallfasten" });

            migrationBuilder.UpdateData(
                schema: "System",
                table: "Diets",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Key", "Name" },
                values: new object[] { "vegetarian_milk", "Vegetarisch mit Milch, Ei, Fisch" });

            migrationBuilder.UpdateData(
                schema: "System",
                table: "Diets",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Key", "Name" },
                values: new object[] { "lactose_free", "Laktosefrei" });

            migrationBuilder.UpdateData(
                schema: "System",
                table: "Diets",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Key", "Name" },
                values: new object[] { "pescetarian", "Vegetarisch mit Fisch" });

            migrationBuilder.UpdateData(
                schema: "System",
                table: "Diets",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Key", "Name" },
                values: new object[] { "low_fructose", "Frustosearm" });

            migrationBuilder.UpdateData(
                schema: "System",
                table: "Motivations",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Anti-Aging");

            migrationBuilder.UpdateData(
                schema: "System",
                table: "Motivations",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Healthier lifestyle");

            migrationBuilder.UpdateData(
                schema: "System",
                table: "Motivations",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Reduce weight");

            migrationBuilder.UpdateData(
                schema: "System",
                table: "Motivations",
                keyColumn: "Id",
                keyValue: 4,
                column: "Name",
                value: "Feel better");
        }
    }
}
