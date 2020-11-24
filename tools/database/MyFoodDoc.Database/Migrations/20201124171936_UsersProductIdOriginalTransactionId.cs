using Microsoft.EntityFrameworkCore.Migrations;

namespace MyFoodDoc.Database.Migrations
{
    public partial class UsersProductIdOriginalTransactionId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_UserName_Gender",
                schema: "User",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "OriginalTransactionId",
                schema: "User",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductId",
                schema: "User",
                table: "Users",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_ProductId_OriginalTransactionId",
                schema: "User",
                table: "Users",
                columns: new[] { "ProductId", "OriginalTransactionId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_ProductId_OriginalTransactionId",
                schema: "User",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "OriginalTransactionId",
                schema: "User",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ProductId",
                schema: "User",
                table: "Users");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserName_Gender",
                schema: "User",
                table: "Users",
                columns: new[] { "UserName", "Gender" });
        }
    }
}
