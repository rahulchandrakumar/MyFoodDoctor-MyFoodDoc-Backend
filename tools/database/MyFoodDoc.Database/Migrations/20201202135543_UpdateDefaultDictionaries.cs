using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyFoodDoc.Database.Migrations
{
    public partial class UpdateDefaultDictionaries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "System",
                table: "Diets",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                schema: "System",
                table: "Diets",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                schema: "System",
                table: "Indications",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.UpdateData(
                schema: "System",
                table: "Indications",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Key", "Name" },
                values: new object[] { "diabetes_type_1", "Diabetes Typ 1" });

            migrationBuilder.UpdateData(
                schema: "System",
                table: "Indications",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Key", "Name" },
                values: new object[] { "diabetes_type_2", "Diabetes Typ 2" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "System",
                table: "Diets",
                columns: new[] { "Id", "Created", "Key", "LastModified", "Name" },
                values: new object[,]
                {
                    { 6, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "fructose_low", null, "Fruktosearm" },
                    { 7, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "gluten_free", null, "Glutenfrei" }
                });

            migrationBuilder.UpdateData(
                schema: "System",
                table: "Indications",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Key", "Name" },
                values: new object[] { "gout", "Gicht" });

            migrationBuilder.UpdateData(
                schema: "System",
                table: "Indications",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Key", "Name" },
                values: new object[] { "diabetes_type_1", "Diabetes Typ 1" });

            migrationBuilder.InsertData(
                schema: "System",
                table: "Indications",
                columns: new[] { "Id", "Created", "Key", "LastModified", "Name" },
                values: new object[] { 5, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "diabetes_type_2", null, "Diabetes Typ 2" });
        }
    }
}
