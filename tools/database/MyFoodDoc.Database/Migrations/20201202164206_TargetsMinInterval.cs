using Microsoft.EntityFrameworkCore.Migrations;

namespace MyFoodDoc.Database.Migrations
{
    public partial class TargetsMinInterval : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MinInterval",
                schema: "System",
                table: "Targets",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MinInterval",
                schema: "System",
                table: "Targets");
        }
    }
}
