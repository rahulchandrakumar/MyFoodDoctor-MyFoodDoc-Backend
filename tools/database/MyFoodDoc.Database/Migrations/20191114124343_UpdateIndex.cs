using Microsoft.EntityFrameworkCore.Migrations;

namespace MyFoodDoc.Database.Migrations
{
    public partial class UpdateIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AbdonimalGirthHistory_Users_UserId",
                schema: "User",
                table: "AbdonimalGirthHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_BloodSugarLevelHistory_Users_UserId",
                schema: "User",
                table: "BloodSugarLevelHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_WeightHistory_Users_UserId",
                schema: "User",
                table: "WeightHistory");

            migrationBuilder.AddForeignKey(
                name: "FK_AbdonimalGirthHistory_Users_UserId",
                schema: "User",
                table: "AbdonimalGirthHistory",
                column: "UserId",
                principalSchema: "User",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BloodSugarLevelHistory_Users_UserId",
                schema: "User",
                table: "BloodSugarLevelHistory",
                column: "UserId",
                principalSchema: "User",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WeightHistory_Users_UserId",
                schema: "User",
                table: "WeightHistory",
                column: "UserId",
                principalSchema: "User",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AbdonimalGirthHistory_Users_UserId",
                schema: "User",
                table: "AbdonimalGirthHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_BloodSugarLevelHistory_Users_UserId",
                schema: "User",
                table: "BloodSugarLevelHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_WeightHistory_Users_UserId",
                schema: "User",
                table: "WeightHistory");

            migrationBuilder.AddForeignKey(
                name: "FK_AbdonimalGirthHistory_Users_UserId",
                schema: "User",
                table: "AbdonimalGirthHistory",
                column: "UserId",
                principalSchema: "User",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BloodSugarLevelHistory_Users_UserId",
                schema: "User",
                table: "BloodSugarLevelHistory",
                column: "UserId",
                principalSchema: "User",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WeightHistory_Users_UserId",
                schema: "User",
                table: "WeightHistory",
                column: "UserId",
                principalSchema: "User",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
