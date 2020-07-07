using Microsoft.EntityFrameworkCore.Migrations;

namespace MyFoodDoc.Database.Migrations
{
    public partial class OptimizationAreaLimits : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "LowerLimit",
                schema: "System",
                table: "OptimizationAreas",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Optimal",
                schema: "System",
                table: "OptimizationAreas",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "UpperLimit",
                schema: "System",
                table: "OptimizationAreas",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.UpdateData(
                schema: "System",
                table: "OptimizationAreas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "LowerLimit", "Optimal" },
                values: new object[] { 400m, 500m });

            migrationBuilder.UpdateData(
                schema: "System",
                table: "OptimizationAreas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Optimal", "UpperLimit" },
                values: new object[] { 30m, 40m });

            migrationBuilder.UpdateData(
                schema: "System",
                table: "OptimizationAreas",
                keyColumn: "Id",
                keyValue: 3,
                column: "Optimal",
                value: 1m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LowerLimit",
                schema: "System",
                table: "OptimizationAreas");

            migrationBuilder.DropColumn(
                name: "Optimal",
                schema: "System",
                table: "OptimizationAreas");

            migrationBuilder.DropColumn(
                name: "UpperLimit",
                schema: "System",
                table: "OptimizationAreas");
        }
    }
}
