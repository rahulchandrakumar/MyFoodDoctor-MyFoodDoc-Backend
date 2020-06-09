using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyFoodDoc.Database.Migrations
{
    public partial class UsersHasSubscription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasSubscription",
                schema: "User",
                table: "Users",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "HasSubscriptionUpdated",
                schema: "User",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasSubscription",
                schema: "User",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "HasSubscriptionUpdated",
                schema: "User",
                table: "Users");
        }
    }
}
