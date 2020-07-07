using Microsoft.EntityFrameworkCore.Migrations;

namespace MyFoodDoc.Database.Migrations
{
    public partial class MethodsAddImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ImageId",
                schema: "System",
                table: "Methods",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Methods_ImageId",
                schema: "System",
                table: "Methods",
                column: "ImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Methods_Images_ImageId",
                schema: "System",
                table: "Methods",
                column: "ImageId",
                principalSchema: "System",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Methods_Images_ImageId",
                schema: "System",
                table: "Methods");

            migrationBuilder.DropIndex(
                name: "IX_Methods_ImageId",
                schema: "System",
                table: "Methods");

            migrationBuilder.DropColumn(
                name: "ImageId",
                schema: "System",
                table: "Methods");
        }
    }
}
