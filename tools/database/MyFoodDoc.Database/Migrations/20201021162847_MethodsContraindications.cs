using Microsoft.EntityFrameworkCore.Migrations;

namespace MyFoodDoc.Database.Migrations
{
    public partial class MethodsContraindications : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsContraindication",
                schema: "System",
                table: "MotivationMethods",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsContraindication",
                schema: "System",
                table: "IndicationMethods",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsContraindication",
                schema: "System",
                table: "DietMethods",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsContraindication",
                schema: "System",
                table: "MotivationMethods");

            migrationBuilder.DropColumn(
                name: "IsContraindication",
                schema: "System",
                table: "IndicationMethods");

            migrationBuilder.DropColumn(
                name: "IsContraindication",
                schema: "System",
                table: "DietMethods");
        }
    }
}
