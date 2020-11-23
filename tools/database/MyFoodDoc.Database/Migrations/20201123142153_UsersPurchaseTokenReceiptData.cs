using Microsoft.EntityFrameworkCore.Migrations;

namespace MyFoodDoc.Database.Migrations
{
    public partial class UsersPurchaseTokenReceiptData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_SubscriptionType_SubscriptionToken",
                schema: "User",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SubscriptionToken",
                schema: "User",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "PurchaseToken",
                schema: "User",
                table: "Users",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReceiptData",
                schema: "User",
                table: "Users",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_PurchaseToken",
                schema: "User",
                table: "Users",
                column: "PurchaseToken");

            migrationBuilder.CreateIndex(
                name: "IX_Users_SubscriptionUpdated",
                schema: "User",
                table: "Users",
                column: "SubscriptionUpdated");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_PurchaseToken",
                schema: "User",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_SubscriptionUpdated",
                schema: "User",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PurchaseToken",
                schema: "User",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ReceiptData",
                schema: "User",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "SubscriptionToken",
                schema: "User",
                table: "Users",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_SubscriptionType_SubscriptionToken",
                schema: "User",
                table: "Users",
                columns: new[] { "SubscriptionType", "SubscriptionToken" });
        }
    }
}
