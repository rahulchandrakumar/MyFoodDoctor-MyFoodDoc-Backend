using Microsoft.EntityFrameworkCore.Migrations;

namespace MyFoodDoc.Database.Migrations
{
    public partial class OptimizationAreasDiagramTexts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpperLimit",
                schema: "System",
                table: "OptimizationAreas",
                newName: "LineGraphUpperLimit");

            migrationBuilder.RenameColumn(
                name: "Optimal",
                schema: "System",
                table: "OptimizationAreas",
                newName: "LineGraphOptimal");

            migrationBuilder.RenameColumn(
                name: "LowerLimit",
                schema: "System",
                table: "OptimizationAreas",
                newName: "LineGraphLowerLimit");

            migrationBuilder.AddColumn<string>(
                name: "AboveOptimalLineGraphText",
                schema: "System",
                table: "OptimizationAreas",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AboveOptimalLineGraphTitle",
                schema: "System",
                table: "OptimizationAreas",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AboveOptimalPieChartText",
                schema: "System",
                table: "OptimizationAreas",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AboveOptimalPieChartTitle",
                schema: "System",
                table: "OptimizationAreas",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BelowOptimalLineGraphText",
                schema: "System",
                table: "OptimizationAreas",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BelowOptimalLineGraphTitle",
                schema: "System",
                table: "OptimizationAreas",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BelowOptimalPieChartText",
                schema: "System",
                table: "OptimizationAreas",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BelowOptimalPieChartTitle",
                schema: "System",
                table: "OptimizationAreas",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OptimalLineGraphText",
                schema: "System",
                table: "OptimizationAreas",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OptimalLineGraphTitle",
                schema: "System",
                table: "OptimizationAreas",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OptimalPieChartText",
                schema: "System",
                table: "OptimizationAreas",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OptimalPieChartTitle",
                schema: "System",
                table: "OptimizationAreas",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AboveOptimalLineGraphText",
                schema: "System",
                table: "OptimizationAreas");

            migrationBuilder.DropColumn(
                name: "AboveOptimalLineGraphTitle",
                schema: "System",
                table: "OptimizationAreas");

            migrationBuilder.DropColumn(
                name: "AboveOptimalPieChartText",
                schema: "System",
                table: "OptimizationAreas");

            migrationBuilder.DropColumn(
                name: "AboveOptimalPieChartTitle",
                schema: "System",
                table: "OptimizationAreas");

            migrationBuilder.DropColumn(
                name: "BelowOptimalLineGraphText",
                schema: "System",
                table: "OptimizationAreas");

            migrationBuilder.DropColumn(
                name: "BelowOptimalLineGraphTitle",
                schema: "System",
                table: "OptimizationAreas");

            migrationBuilder.DropColumn(
                name: "BelowOptimalPieChartText",
                schema: "System",
                table: "OptimizationAreas");

            migrationBuilder.DropColumn(
                name: "BelowOptimalPieChartTitle",
                schema: "System",
                table: "OptimizationAreas");

            migrationBuilder.DropColumn(
                name: "OptimalLineGraphText",
                schema: "System",
                table: "OptimizationAreas");

            migrationBuilder.DropColumn(
                name: "OptimalLineGraphTitle",
                schema: "System",
                table: "OptimizationAreas");

            migrationBuilder.DropColumn(
                name: "OptimalPieChartText",
                schema: "System",
                table: "OptimizationAreas");

            migrationBuilder.DropColumn(
                name: "OptimalPieChartTitle",
                schema: "System",
                table: "OptimizationAreas");

            migrationBuilder.RenameColumn(
                name: "LineGraphUpperLimit",
                schema: "System",
                table: "OptimizationAreas",
                newName: "UpperLimit");

            migrationBuilder.RenameColumn(
                name: "LineGraphOptimal",
                schema: "System",
                table: "OptimizationAreas",
                newName: "Optimal");

            migrationBuilder.RenameColumn(
                name: "LineGraphLowerLimit",
                schema: "System",
                table: "OptimizationAreas",
                newName: "LowerLimit");
        }
    }
}
