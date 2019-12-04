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

            migrationBuilder.UpdateData(
                schema: "User",
                table: "Users",
                keyColumn: "Id",
                keyValue: "3ee857ac-26ee-43d8-8f68-76f1ca7bfa9b",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "7ab0095d-dcdf-4c19-a79f-ea4e49386b86", "AQAAAAEAACcQAAAAEAmM0h6jTE6UAJOwjVOFwxfiqtgbWCx6PVMoTGm5EajuNXWZRV9y8j8d+2EIoWZHog==" });

            migrationBuilder.CreateIndex(
                name: "IX_Coupons_InsuranceId",
                schema: "Coupon",
                table: "Coupons",
                column: "InsuranceId");

            migrationBuilder.Sql("UPDATE Coupon.Coupons SET InsuranceId = (SELECT InsuranceId FROM Coupon.Promotions as p WHERE p.Id = PromotionId)");

            migrationBuilder.CreateIndex(
                name: "IX_Coupons_Code_InsuranceId",
                schema: "Coupon",
                table: "Coupons",
                columns: new[] { "Code", "InsuranceId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Coupons_Insurances_InsuranceId",
                schema: "Coupon",
                table: "Coupons",
                column: "InsuranceId",
                principalSchema: "Values",
                principalTable: "Insurances",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coupons_Insurances_InsuranceId",
                schema: "Coupon",
                table: "Coupons");

            migrationBuilder.DropIndex(
                name: "IX_Coupons_InsuranceId",
                schema: "Coupon",
                table: "Coupons");

            migrationBuilder.DropIndex(
                name: "IX_Coupons_Code_InsuranceId",
                schema: "Coupon",
                table: "Coupons");

            migrationBuilder.DropColumn(
                name: "InsuranceId",
                schema: "Coupon",
                table: "Coupons");

            migrationBuilder.UpdateData(
                schema: "User",
                table: "Users",
                keyColumn: "Id",
                keyValue: "3ee857ac-26ee-43d8-8f68-76f1ca7bfa9b",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "b48bbac3-02b1-4b9b-94dd-b95c115f66c2", "AQAAAAEAACcQAAAAEK0gS+06kXikda3vMMDvIvlP95OdKFo0lDoQUxypWGqO/lxRlr2GNLS9li/wzp7S6g==" });
        }
    }
}
