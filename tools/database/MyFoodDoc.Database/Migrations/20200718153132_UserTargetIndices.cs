using Microsoft.EntityFrameworkCore.Migrations;

namespace MyFoodDoc.Database.Migrations
{
    public partial class UserTargetIndices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserTargets_UserId_Created_TargetId",
                schema: "System",
                table: "UserTargets");

            migrationBuilder.DropIndex(
                name: "IX_UserMethodShowHistory_UserId_Date_MethodId",
                schema: "System",
                table: "UserMethodShowHistory");

            migrationBuilder.DropIndex(
                name: "IX_UserMethods_UserId_MethodId_Created",
                schema: "System",
                table: "UserMethods");

            migrationBuilder.CreateIndex(
                name: "IX_UserTargets_UserId_TargetId",
                schema: "System",
                table: "UserTargets",
                columns: new[] { "UserId", "TargetId" });

            migrationBuilder.CreateIndex(
                name: "IX_UserMethodShowHistory_UserId_MethodId",
                schema: "System",
                table: "UserMethodShowHistory",
                columns: new[] { "UserId", "MethodId" });

            migrationBuilder.CreateIndex(
                name: "IX_UserMethods_UserId_MethodId",
                schema: "System",
                table: "UserMethods",
                columns: new[] { "UserId", "MethodId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserTargets_UserId_TargetId",
                schema: "System",
                table: "UserTargets");

            migrationBuilder.DropIndex(
                name: "IX_UserMethodShowHistory_UserId_MethodId",
                schema: "System",
                table: "UserMethodShowHistory");

            migrationBuilder.DropIndex(
                name: "IX_UserMethods_UserId_MethodId",
                schema: "System",
                table: "UserMethods");

            migrationBuilder.CreateIndex(
                name: "IX_UserTargets_UserId_Created_TargetId",
                schema: "System",
                table: "UserTargets",
                columns: new[] { "UserId", "Created", "TargetId" });

            migrationBuilder.CreateIndex(
                name: "IX_UserMethodShowHistory_UserId_Date_MethodId",
                schema: "System",
                table: "UserMethodShowHistory",
                columns: new[] { "UserId", "Date", "MethodId" });

            migrationBuilder.CreateIndex(
                name: "IX_UserMethods_UserId_MethodId_Created",
                schema: "System",
                table: "UserMethods",
                columns: new[] { "UserId", "MethodId", "Created" });
        }
    }
}
