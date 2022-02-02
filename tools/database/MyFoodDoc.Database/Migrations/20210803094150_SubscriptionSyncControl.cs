using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace MyFoodDoc.Database.Migrations
{
    public partial class SubscriptionSyncControl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FirstSynchronized",
                schema: "User",
                table: "GooglePlayStoreSubscriptions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsExpired",
                schema: "User",
                table: "GooglePlayStoreSubscriptions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "FirstSynchronized",
                schema: "User",
                table: "AppStoreSubscriptions",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstSynchronized",
                schema: "User",
                table: "GooglePlayStoreSubscriptions");

            migrationBuilder.DropColumn(
                name: "IsExpired",
                schema: "User",
                table: "GooglePlayStoreSubscriptions");

            migrationBuilder.DropColumn(
                name: "FirstSynchronized",
                schema: "User",
                table: "AppStoreSubscriptions");
        }
    }
}
