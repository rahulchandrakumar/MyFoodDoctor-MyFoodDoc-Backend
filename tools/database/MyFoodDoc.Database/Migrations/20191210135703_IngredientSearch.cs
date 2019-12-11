using Microsoft.EntityFrameworkCore.Migrations;

namespace MyFoodDoc.Database.Migrations
{
    public partial class IngredientSearch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Ingredients_ExternalKey",
                schema: "Food",
                table: "Ingredients");

            migrationBuilder.UpdateData(
                schema: "User",
                table: "Users",
                keyColumn: "Id",
                keyValue: "3ee857ac-26ee-43d8-8f68-76f1ca7bfa9b",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "a1b19c0a-42a3-4050-a7f7-391ec0e0e760", "AQAAAAEAACcQAAAAEBYzVKiS6N4MKsUx05LAiKSLklmMvK3m2Pj8jlNnx5XrHHho+hTazqWbfS2LJxTKAg==" });

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_ExternalKey_Name",
                schema: "Food",
                table: "Ingredients",
                columns: new[] { "ExternalKey", "Name" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Ingredients_ExternalKey_Name",
                schema: "Food",
                table: "Ingredients");

            migrationBuilder.UpdateData(
                schema: "User",
                table: "Users",
                keyColumn: "Id",
                keyValue: "3ee857ac-26ee-43d8-8f68-76f1ca7bfa9b",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "86253e13-b079-40dc-9fa8-0dc185244076", "AQAAAAEAACcQAAAAEOuEFP35Ay5SrRWbwyVgwOtcO/S0fo+x0Mc9Ei9QGGg5O2UdFhCcbe0UQAZLkxZvBQ==" });

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_ExternalKey",
                schema: "Food",
                table: "Ingredients",
                column: "ExternalKey");
        }
    }
}
