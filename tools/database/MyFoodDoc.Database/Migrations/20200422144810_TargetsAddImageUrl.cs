using Microsoft.EntityFrameworkCore.Migrations;

namespace MyFoodDoc.Database.Migrations
{
    public partial class TargetsAddImageUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ImageId",
                schema: "System",
                table: "Targets",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ImageId",
                schema: "System",
                table: "OptimizationAreas",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Targets_ImageId",
                schema: "System",
                table: "Targets",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_OptimizationAreas_ImageId",
                schema: "System",
                table: "OptimizationAreas",
                column: "ImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_OptimizationAreas_Images_ImageId",
                schema: "System",
                table: "OptimizationAreas",
                column: "ImageId",
                principalSchema: "System",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Targets_Images_ImageId",
                schema: "System",
                table: "Targets",
                column: "ImageId",
                principalSchema: "System",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OptimizationAreas_Images_ImageId",
                schema: "System",
                table: "OptimizationAreas");

            migrationBuilder.DropForeignKey(
                name: "FK_Targets_Images_ImageId",
                schema: "System",
                table: "Targets");

            migrationBuilder.DropIndex(
                name: "IX_Targets_ImageId",
                schema: "System",
                table: "Targets");

            migrationBuilder.DropIndex(
                name: "IX_OptimizationAreas_ImageId",
                schema: "System",
                table: "OptimizationAreas");

            migrationBuilder.DropColumn(
                name: "ImageId",
                schema: "System",
                table: "Targets");

            migrationBuilder.DropColumn(
                name: "ImageId",
                schema: "System",
                table: "OptimizationAreas");
        }
    }
}
