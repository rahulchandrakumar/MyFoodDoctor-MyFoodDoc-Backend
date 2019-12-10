using Microsoft.EntityFrameworkCore.Migrations;

namespace MyFoodDoc.Database.Migrations
{
    public partial class IngredientAmount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Amount",
                schema: "Food",
                table: "Ingredients",
                nullable: true);

            migrationBuilder.UpdateData(
                schema: "User",
                table: "Users",
                keyColumn: "Id",
                keyValue: "3ee857ac-26ee-43d8-8f68-76f1ca7bfa9b",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "86253e13-b079-40dc-9fa8-0dc185244076", "AQAAAAEAACcQAAAAEOuEFP35Ay5SrRWbwyVgwOtcO/S0fo+x0Mc9Ei9QGGg5O2UdFhCcbe0UQAZLkxZvBQ==" });

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_Amount",
                schema: "Food",
                table: "Ingredients",
                column: "Amount");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Ingredients_Amount",
                schema: "Food",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "Amount",
                schema: "Food",
                table: "Ingredients");

            migrationBuilder.UpdateData(
                schema: "User",
                table: "Users",
                keyColumn: "Id",
                keyValue: "3ee857ac-26ee-43d8-8f68-76f1ca7bfa9b",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "5c4aa358-0615-4303-85bc-1b33a9564984", "AQAAAAEAACcQAAAAECi09GWAEqVArlW3eTnGJ6Oyjpj396YfN1m8slVmpFPiMkKghPCQjzdmVIVsHbRQaw==" });
        }
    }
}
