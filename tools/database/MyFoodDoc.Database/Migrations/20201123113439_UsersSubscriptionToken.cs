using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyFoodDoc.Database.Migrations
{
    public partial class UsersSubscriptionToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubscriptionExpirationDate",
                schema: "User",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SubscriptionExpirationDateUpdated",
                schema: "User",
                table: "Users");

            migrationBuilder.AddColumn<bool>(
                name: "HasValidSubscription",
                schema: "User",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubscriptionId",
                schema: "User",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubscriptionToken",
                schema: "User",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubscriptionType",
                schema: "User",
                table: "Users",
                maxLength: 15,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SubscriptionUpdated",
                schema: "User",
                table: "Users",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_SubscriptionType_SubscriptionToken",
                schema: "User",
                table: "Users",
                columns: new[] { "SubscriptionType", "SubscriptionToken" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_SubscriptionType_SubscriptionToken",
                schema: "User",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "HasValidSubscription",
                schema: "User",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SubscriptionId",
                schema: "User",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SubscriptionToken",
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

            migrationBuilder.AddColumn<DateTime>(
                name: "SubscriptionExpirationDate",
                schema: "User",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SubscriptionExpirationDateUpdated",
                schema: "User",
                table: "Users",
                type: "datetime2",
                nullable: true);
        }
    }
}
