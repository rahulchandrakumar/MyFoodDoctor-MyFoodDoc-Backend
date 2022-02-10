using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace MyFoodDoc.Database.Migrations
{
    public partial class Promotions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coupons_Insurances_InsuranceId",
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

            migrationBuilder.DropIndex(
                name: "IX_Coupons_InsuranceId",
                schema: "Coupon",
                table: "Coupons");

            migrationBuilder.DropColumn(
                name: "Expiry",
                schema: "Coupon",
                table: "Coupons");

            migrationBuilder.DropColumn(
                name: "InsuranceId",
                schema: "Coupon",
                table: "Coupons");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "Coupon",
                table: "Coupons");

            migrationBuilder.AddColumn<int>(
                name: "PromotionId",
                schema: "Coupon",
                table: "Coupons",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Promotions",
                schema: "Coupon",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true),
                    Title = table.Column<string>(maxLength: 100, nullable: true),
                    InsuranceId = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Promotions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Promotions_Insurances_InsuranceId",
                        column: x => x.InsuranceId,
                        principalSchema: "Values",
                        principalTable: "Insurances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Coupons_PromotionId",
                schema: "Coupon",
                table: "Coupons",
                column: "PromotionId");

            migrationBuilder.CreateIndex(
                name: "IX_Promotions_InsuranceId",
                schema: "Coupon",
                table: "Promotions",
                column: "InsuranceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Coupons_Promotions_PromotionId",
                schema: "Coupon",
                table: "Coupons",
                column: "PromotionId",
                principalSchema: "Coupon",
                principalTable: "Promotions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
                name: "FK_Coupons_Promotions_PromotionId",
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
                name: "Promotions",
                schema: "Coupon");

            migrationBuilder.DropIndex(
                name: "IX_Coupons_PromotionId",
                schema: "Coupon",
                table: "Coupons");

            migrationBuilder.DropColumn(
                name: "PromotionId",
                schema: "Coupon",
                table: "Coupons");

            migrationBuilder.AddColumn<DateTime>(
                name: "Expiry",
                schema: "Coupon",
                table: "Coupons",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InsuranceId",
                schema: "Coupon",
                table: "Coupons",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "Coupon",
                table: "Coupons",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Coupons_InsuranceId",
                schema: "Coupon",
                table: "Coupons",
                column: "InsuranceId");

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
