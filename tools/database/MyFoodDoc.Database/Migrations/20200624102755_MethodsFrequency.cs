using Microsoft.EntityFrameworkCore.Migrations;

namespace MyFoodDoc.Database.Migrations
{
    public partial class MethodsFrequency : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "DecimalValue",
                schema: "System",
                table: "UserMethods",
                type: "decimal(4,1)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IntegerValue",
                schema: "System",
                table: "UserMethods",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Frequency",
                schema: "System",
                table: "Methods",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FrequencyPeriod",
                schema: "System",
                table: "Methods",
                maxLength: 5,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DecimalValue",
                schema: "System",
                table: "UserMethods");

            migrationBuilder.DropColumn(
                name: "IntegerValue",
                schema: "System",
                table: "UserMethods");

            migrationBuilder.DropColumn(
                name: "Frequency",
                schema: "System",
                table: "Methods");

            migrationBuilder.DropColumn(
                name: "FrequencyPeriod",
                schema: "System",
                table: "Methods");
        }
    }
}
