using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace MyFoodDoc.Database.Migrations
{
    public partial class OptimizationAreaSnacking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TimeIntervalDay",
                schema: "System",
                table: "Methods",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TimeIntervalNight",
                schema: "System",
                table: "Methods",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MethodTexts",
                schema: "System",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MethodId = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Text = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MethodTexts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MethodTexts_Methods_MethodId",
                        column: x => x.MethodId,
                        principalSchema: "System",
                        principalTable: "Methods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "System",
                table: "OptimizationAreas",
                columns: new[] { "Id", "AboveOptimalLineGraphText", "AboveOptimalLineGraphTitle", "AboveOptimalPieChartText", "AboveOptimalPieChartTitle", "BelowOptimalLineGraphText", "BelowOptimalLineGraphTitle", "BelowOptimalPieChartText", "BelowOptimalPieChartTitle", "Created", "ImageId", "Key", "LastModified", "LineGraphLowerLimit", "LineGraphOptimal", "LineGraphUpperLimit", "Name", "OptimalLineGraphText", "OptimalLineGraphTitle", "OptimalPieChartText", "OptimalPieChartTitle", "Text" },
                values: new object[] { 4, null, null, null, null, null, null, null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "snacking", null, null, null, null, "Snacking", null, null, null, null, "Snacking" });

            migrationBuilder.CreateIndex(
                name: "IX_MethodTexts_MethodId",
                schema: "System",
                table: "MethodTexts",
                column: "MethodId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MethodTexts",
                schema: "System");

            migrationBuilder.DeleteData(
                schema: "System",
                table: "OptimizationAreas",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DropColumn(
                name: "TimeIntervalDay",
                schema: "System",
                table: "Methods");

            migrationBuilder.DropColumn(
                name: "TimeIntervalNight",
                schema: "System",
                table: "Methods");
        }
    }
}
