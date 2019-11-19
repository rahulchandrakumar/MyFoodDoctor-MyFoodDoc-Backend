using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyFoodDoc.Database.Migrations
{
    public partial class Images : Migration
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

            migrationBuilder.DropIndex(
                name: "IX_Meals_UserId",
                schema: "Diary",
                table: "Meals");

            migrationBuilder.DropIndex(
                name: "IX_Meals_Date_Type",
                schema: "Diary",
                table: "Meals");

            migrationBuilder.AddColumn<int>(
                name: "InsuranceId",
                schema: "Coupon",
                table: "Coupons",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "RedeemedBy",
                schema: "Coupon",
                table: "Coupons",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Images",
                schema: "System",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true),
                    Url = table.Column<string>(maxLength: 2083, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                });

            migrationBuilder.UpdateData(
                schema: "User",
                table: "Users",
                keyColumn: "Id",
                keyValue: "3ee857ac-26ee-43d8-8f68-76f1ca7bfa9b",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "984d76b8-a7a3-4b51-aae1-2142b785dbfb", "AQAAAAEAACcQAAAAEJ6ydXqrzc+kxqUY0ztlAgyB0ADFLmvWlUSEZQadER6SwrMaW8sCyL0Sky81GoPKeg==" });

            migrationBuilder.CreateIndex(
                name: "IX_Meals_UserId_Date_Type",
                schema: "Diary",
                table: "Meals",
                columns: new[] { "UserId", "Date", "Type" },
                unique: true,
                filter: "Type IN ('Breakfast', 'Lunch', 'Dinner')");

            migrationBuilder.CreateIndex(
                name: "IX_Coupons_InsuranceId",
                schema: "Coupon",
                table: "Coupons",
                column: "InsuranceId");

            migrationBuilder.CreateIndex(
                name: "IX_Coupons_RedeemedBy",
                schema: "Coupon",
                table: "Coupons",
                column: "RedeemedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Coupons_Insurances_InsuranceId",
                schema: "Coupon",
                table: "Coupons",
                column: "InsuranceId",
                principalSchema: "Values",
                principalTable: "Insurances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Coupons_Users_RedeemedBy",
                schema: "Coupon",
                table: "Coupons",
                column: "RedeemedBy",
                principalSchema: "User",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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
                name: "FK_Coupons_Insurances_InsuranceId",
                schema: "Coupon",
                table: "Coupons");

            migrationBuilder.DropForeignKey(
                name: "FK_Coupons_Users_RedeemedBy",
                schema: "Coupon",
                table: "Coupons");

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

            migrationBuilder.DropTable(
                name: "Images",
                schema: "System");

            migrationBuilder.DropIndex(
                name: "IX_Meals_UserId_Date_Type",
                schema: "Diary",
                table: "Meals");

            migrationBuilder.DropIndex(
                name: "IX_Coupons_InsuranceId",
                schema: "Coupon",
                table: "Coupons");

            migrationBuilder.DropIndex(
                name: "IX_Coupons_RedeemedBy",
                schema: "Coupon",
                table: "Coupons");

            migrationBuilder.DropColumn(
                name: "InsuranceId",
                schema: "Coupon",
                table: "Coupons");

            migrationBuilder.DropColumn(
                name: "RedeemedBy",
                schema: "Coupon",
                table: "Coupons");

            migrationBuilder.UpdateData(
                schema: "User",
                table: "Users",
                keyColumn: "Id",
                keyValue: "3ee857ac-26ee-43d8-8f68-76f1ca7bfa9b",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "52a2cf89-f985-4797-bf8c-1dae8d84a407", "AQAAAAEAACcQAAAAEPTn7XLFIsZ3OwU1MLTCZbkLNBZzW3vfyyK1XiZvTSO3UgE/dXVrKyjOxX01IL1CBQ==" });

            migrationBuilder.CreateIndex(
                name: "IX_Meals_UserId",
                schema: "Diary",
                table: "Meals",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Meals_Date_Type",
                schema: "Diary",
                table: "Meals",
                columns: new[] { "Date", "Type" },
                unique: true,
                filter: "Type = 'Snack'");

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
