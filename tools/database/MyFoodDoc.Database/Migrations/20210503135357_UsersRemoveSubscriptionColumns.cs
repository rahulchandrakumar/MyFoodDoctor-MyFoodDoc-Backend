using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyFoodDoc.Database.Migrations
{
    public partial class UsersRemoveSubscriptionColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_ProductId_OriginalTransactionId",
                schema: "User",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_PurchaseToken",
                schema: "User",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_SubscriptionUpdated",
                schema: "User",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "HasValidSubscription",
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

            migrationBuilder.DropColumn(
                name: "PurchaseToken",
                schema: "User",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ReceiptData",
                schema: "User",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SubscriptionId",
                schema: "User",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SubscriptionType",
                schema: "User",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SubscriptionUpdated",
                schema: "User",
                table: "Users");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasValidSubscription",
                schema: "User",
                table: "Users",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OriginalTransactionId",
                schema: "User",
                table: "Users",
                type: "varchar(1000)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductId",
                schema: "User",
                table: "Users",
                type: "varchar(1000)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PurchaseToken",
                schema: "User",
                table: "Users",
                type: "varchar(1000)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReceiptData",
                schema: "User",
                table: "Users",
                type: "varchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubscriptionId",
                schema: "User",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubscriptionType",
                schema: "User",
                table: "Users",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SubscriptionUpdated",
                schema: "User",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_ProductId_OriginalTransactionId",
                schema: "User",
                table: "Users",
                columns: new[] { "ProductId", "OriginalTransactionId" });

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
    }
}
