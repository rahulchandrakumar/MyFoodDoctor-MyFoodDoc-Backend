using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyFoodDoc.Database.Migrations
{
    public partial class UserSubscriptions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppStoreSubscriptions",
                schema: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubscriptionId = table.Column<string>(type: "varchar(1000)", nullable: false),
                    PurchaseToken = table.Column<string>(type: "varchar(1000)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    LastSynchronized = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    IsValid = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppStoreSubscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppStoreSubscriptions_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "User",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GooglePlayStoreSubscriptions",
                schema: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReceiptData = table.Column<string>(type: "varchar(max)", nullable: false),
                    ProductId = table.Column<string>(type: "varchar(1000)", nullable: false),
                    OriginalTransactionId = table.Column<string>(type: "varchar(1000)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    LastSynchronized = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    IsValid = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GooglePlayStoreSubscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GooglePlayStoreSubscriptions_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "User",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppStoreSubscriptions_LastSynchronized",
                schema: "User",
                table: "AppStoreSubscriptions",
                column: "LastSynchronized");

            migrationBuilder.CreateIndex(
                name: "IX_AppStoreSubscriptions_PurchaseToken",
                schema: "User",
                table: "AppStoreSubscriptions",
                column: "PurchaseToken");

            migrationBuilder.CreateIndex(
                name: "IX_AppStoreSubscriptions_UserId",
                schema: "User",
                table: "AppStoreSubscriptions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_GooglePlayStoreSubscriptions_LastSynchronized",
                schema: "User",
                table: "GooglePlayStoreSubscriptions",
                column: "LastSynchronized");

            migrationBuilder.CreateIndex(
                name: "IX_GooglePlayStoreSubscriptions_ProductId_OriginalTransactionId",
                schema: "User",
                table: "GooglePlayStoreSubscriptions",
                columns: new[] { "ProductId", "OriginalTransactionId" });

            migrationBuilder.CreateIndex(
                name: "IX_GooglePlayStoreSubscriptions_UserId",
                schema: "User",
                table: "GooglePlayStoreSubscriptions",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppStoreSubscriptions",
                schema: "User");

            migrationBuilder.DropTable(
                name: "GooglePlayStoreSubscriptions",
                schema: "User");
        }
    }
}
