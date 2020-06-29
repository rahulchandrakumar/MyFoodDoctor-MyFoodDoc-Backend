using Microsoft.EntityFrameworkCore.Migrations;

namespace MyFoodDoc.Database.Migrations
{
    public partial class AbdominalGirthRename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AbdonimalGirthHistory_Users_UserId",
                schema: "User",
                table: "AbdonimalGirthHistory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AbdonimalGirthHistory",
                schema: "User",
                table: "AbdonimalGirthHistory");

            migrationBuilder.RenameTable(
                name: "AbdonimalGirthHistory",
                schema: "User",
                newName: "AbdominalGirthHistory",
                newSchema: "User");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AbdominalGirthHistory",
                schema: "User",
                table: "AbdominalGirthHistory",
                columns: new[] { "UserId", "Date" });

            migrationBuilder.AddForeignKey(
                name: "FK_AbdominalGirthHistory_Users_UserId",
                schema: "User",
                table: "AbdominalGirthHistory",
                column: "UserId",
                principalSchema: "User",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AbdominalGirthHistory_Users_UserId",
                schema: "User",
                table: "AbdominalGirthHistory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AbdominalGirthHistory",
                schema: "User",
                table: "AbdominalGirthHistory");

            migrationBuilder.RenameTable(
                name: "AbdominalGirthHistory",
                schema: "User",
                newName: "AbdonimalGirthHistory",
                newSchema: "User");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AbdonimalGirthHistory",
                schema: "User",
                table: "AbdonimalGirthHistory",
                columns: new[] { "UserId", "Date" });

            migrationBuilder.AddForeignKey(
                name: "FK_AbdonimalGirthHistory_Users_UserId",
                schema: "User",
                table: "AbdonimalGirthHistory",
                column: "UserId",
                principalSchema: "User",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
