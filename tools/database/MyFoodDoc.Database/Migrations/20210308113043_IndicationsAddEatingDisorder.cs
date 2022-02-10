using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace MyFoodDoc.Database.Migrations
{
    public partial class IndicationsAddEatingDisorder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "System",
                table: "Indications",
                columns: new[] { "Id", "Created", "Key", "LastModified", "Name" },
                values: new object[] { 5, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "eating_disorder", null, "Essstörungen oder psychiatrische Grunderkrankung" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "System",
                table: "Indications",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
