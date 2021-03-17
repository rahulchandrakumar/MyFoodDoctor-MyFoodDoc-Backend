using Microsoft.EntityFrameworkCore.Migrations;

namespace MyFoodDoc.Database.Migrations
{
    public partial class PsychogrammNewFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Characterization",
                schema: "Psychogramm",
                table: "Scales",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Reason",
                schema: "Psychogramm",
                table: "Scales",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Treatment",
                schema: "Psychogramm",
                table: "Scales",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TypeCode",
                schema: "Psychogramm",
                table: "Scales",
                type: "nvarchar(6)",
                maxLength: 6,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TypeText",
                schema: "Psychogramm",
                table: "Scales",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TypeTitle",
                schema: "Psychogramm",
                table: "Scales",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Extra",
                schema: "Psychogramm",
                table: "Questions",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Characterization",
                schema: "Psychogramm",
                table: "Scales");

            migrationBuilder.DropColumn(
                name: "Reason",
                schema: "Psychogramm",
                table: "Scales");

            migrationBuilder.DropColumn(
                name: "Treatment",
                schema: "Psychogramm",
                table: "Scales");

            migrationBuilder.DropColumn(
                name: "TypeCode",
                schema: "Psychogramm",
                table: "Scales");

            migrationBuilder.DropColumn(
                name: "TypeText",
                schema: "Psychogramm",
                table: "Scales");

            migrationBuilder.DropColumn(
                name: "TypeTitle",
                schema: "Psychogramm",
                table: "Scales");

            migrationBuilder.DropColumn(
                name: "Extra",
                schema: "Psychogramm",
                table: "Questions");
        }
    }
}
