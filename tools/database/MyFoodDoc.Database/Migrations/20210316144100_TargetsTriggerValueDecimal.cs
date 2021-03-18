using Microsoft.EntityFrameworkCore.Migrations;

namespace MyFoodDoc.Database.Migrations
{
    public partial class TargetsTriggerValueDecimal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "TriggerValue",
                schema: "System",
                table: "Targets",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TriggerValue",
                schema: "System",
                table: "Targets",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }
    }
}
