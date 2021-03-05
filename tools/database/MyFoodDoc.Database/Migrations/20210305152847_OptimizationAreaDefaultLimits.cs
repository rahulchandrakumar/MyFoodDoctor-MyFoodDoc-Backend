using Microsoft.EntityFrameworkCore.Migrations;

namespace MyFoodDoc.Database.Migrations
{
    public partial class OptimizationAreaDefaultLimits : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "System",
                table: "OptimizationAreas",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "LineGraphLowerLimit", "LineGraphUpperLimit" },
                values: new object[] { 0.8m, 1.5m });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "System",
                table: "OptimizationAreas",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "LineGraphLowerLimit", "LineGraphUpperLimit" },
                values: new object[] { null, null });
        }
    }
}
