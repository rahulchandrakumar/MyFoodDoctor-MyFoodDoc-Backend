using Microsoft.EntityFrameworkCore.Migrations;

namespace MyFoodDoc.Database.Migrations
{
    public partial class UniqueCouponCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InsuranceId",
                schema: "Coupon",
                table: "Coupons",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql("UPDATE [Coupon].[Coupons] SET [InsuranceId] = p.[InsuranceId] FROM [Coupon].[Promotions] as p WHERE [PromotionId]=p.[Id]");

            migrationBuilder.DropForeignKey(
                name: "FK_Coupons_Promotions_PromotionId",
                schema: "Coupon",
                table: "Coupons");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Promotions",
                schema: "Coupon",
                table: "Promotions");

            migrationBuilder.DropIndex(
                name: "IX_Coupons_PromotionId",
                schema: "Coupon",
                table: "Coupons");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Promotions",
                schema: "Coupon",
                table: "Promotions",
                columns: new[] { "Id", "InsuranceId" });

            migrationBuilder.UpdateData(
                schema: "User",
                table: "Users",
                keyColumn: "Id",
                keyValue: "3ee857ac-26ee-43d8-8f68-76f1ca7bfa9b",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "061fce92-6b05-4fcf-9378-4551af35d604", "AQAAAAEAACcQAAAAEKFb1tcxc48UHhaiaD1gdDAP5EqRmnVFkWy1EAp9njVf58I9AYtrSGtr2XirDaQhPA==" });

            migrationBuilder.CreateIndex(
                name: "IX_Coupons_Code_InsuranceId",
                schema: "Coupon",
                table: "Coupons",
                columns: new[] { "Code", "InsuranceId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Coupons_PromotionId_InsuranceId",
                schema: "Coupon",
                table: "Coupons",
                columns: new[] { "PromotionId", "InsuranceId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Coupons_Promotions_PromotionId_InsuranceId",
                schema: "Coupon",
                table: "Coupons",
                columns: new[] { "PromotionId", "InsuranceId" },
                principalSchema: "Coupon",
                principalTable: "Promotions",
                principalColumns: new[] { "Id", "InsuranceId" },
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coupons_Promotions_PromotionId_InsuranceId",
                schema: "Coupon",
                table: "Coupons");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Promotions",
                schema: "Coupon",
                table: "Promotions");

            migrationBuilder.DropIndex(
                name: "IX_Coupons_Code_InsuranceId",
                schema: "Coupon",
                table: "Coupons");

            migrationBuilder.DropIndex(
                name: "IX_Coupons_PromotionId_InsuranceId",
                schema: "Coupon",
                table: "Coupons");

            migrationBuilder.DropColumn(
                name: "InsuranceId",
                schema: "Coupon",
                table: "Coupons");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Promotions",
                schema: "Coupon",
                table: "Promotions",
                column: "Id");

            migrationBuilder.UpdateData(
                schema: "User",
                table: "Users",
                keyColumn: "Id",
                keyValue: "3ee857ac-26ee-43d8-8f68-76f1ca7bfa9b",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "b48bbac3-02b1-4b9b-94dd-b95c115f66c2", "AQAAAAEAACcQAAAAEK0gS+06kXikda3vMMDvIvlP95OdKFo0lDoQUxypWGqO/lxRlr2GNLS9li/wzp7S6g==" });

            migrationBuilder.CreateIndex(
                name: "IX_Coupons_PromotionId",
                schema: "Coupon",
                table: "Coupons",
                column: "PromotionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Coupons_Promotions_PromotionId",
                schema: "Coupon",
                table: "Coupons",
                column: "PromotionId",
                principalSchema: "Coupon",
                principalTable: "Promotions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
