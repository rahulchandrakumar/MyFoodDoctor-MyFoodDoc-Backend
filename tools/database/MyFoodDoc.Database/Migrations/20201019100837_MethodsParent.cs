using Microsoft.EntityFrameworkCore.Migrations;

namespace MyFoodDoc.Database.Migrations
{
    public partial class MethodsParent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                schema: "System",
                table: "Methods",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Methods_ParentId",
                schema: "System",
                table: "Methods",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Methods_Methods_ParentId",
                schema: "System",
                table: "Methods",
                column: "ParentId",
                principalSchema: "System",
                principalTable: "Methods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Methods_Methods_ParentId",
                schema: "System",
                table: "Methods");

            migrationBuilder.DropIndex(
                name: "IX_Methods_ParentId",
                schema: "System",
                table: "Methods");

            migrationBuilder.DropColumn(
                name: "ParentId",
                schema: "System",
                table: "Methods");
        }
    }
}
