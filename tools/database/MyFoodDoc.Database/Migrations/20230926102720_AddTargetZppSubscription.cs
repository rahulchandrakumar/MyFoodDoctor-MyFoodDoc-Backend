using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyFoodDoc.Database.Migrations
{
    public partial class AddTargetZppSubscription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ZppSubscription",
                schema: "System",
                table: "Targets",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ZppSubscription",
                schema: "System",
                table: "Targets");
        }
    }
}
