using Microsoft.EntityFrameworkCore.Migrations;

namespace MyFoodDoc.Database.Migrations
{
    public partial class UniqueCouponCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "User",
                table: "Users",
                keyColumn: "Id",
                keyValue: "3ee857ac-26ee-43d8-8f68-76f1ca7bfa9b",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "534dbe5a-91ab-45c7-9b69-bf92f326a14f", "AQAAAAEAACcQAAAAEJIF1Yr6Axt0dLLYZlQ1zPozREk5vss7QiCqImIyMSQa2rL+t+MpEr4M7uFVNbhKhA==" });

            migrationBuilder.CreateIndex(
                name: "IX_Coupons_Code",
                schema: "Coupon",
                table: "Coupons",
                column: "Code");

            migrationBuilder.Sql(
                @"CREATE VIEW Coupon.CodeInsuranceUnique
                    WITH SCHEMABINDING
                    AS  
                        SELECT c.Code, p.InsuranceId
                        FROM Coupon.Coupons c, Coupon.Promotions p
                        WHERE c.PromotionId = p.Id;
                  GO
                  CREATE UNIQUE CLUSTERED INDEX IDX_CodeInsuranceUnique ON Coupon.CodeInsuranceUnique (Code, InsuranceId);  
                ");            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Coupons_Code",
                schema: "Coupon",
                table: "Coupons");

            migrationBuilder.UpdateData(
                schema: "User",
                table: "Users",
                keyColumn: "Id",
                keyValue: "3ee857ac-26ee-43d8-8f68-76f1ca7bfa9b",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "b48bbac3-02b1-4b9b-94dd-b95c115f66c2", "AQAAAAEAACcQAAAAEK0gS+06kXikda3vMMDvIvlP95OdKFo0lDoQUxypWGqO/lxRlr2GNLS9li/wzp7S6g==" });

            migrationBuilder.Sql("DROP VIEW Coupons.CodeInsuranceUnique");
        }
    }
}
