using Microsoft.EntityFrameworkCore.Migrations;

namespace MyFoodDoc.Database.Migrations
{
    public partial class SearchIndexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Value",
                schema: "User",
                table: "WeightHistory",
                type: "decimal(4,1)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.UpdateData(
                schema: "User",
                table: "Users",
                keyColumn: "Id",
                keyValue: "3ee857ac-26ee-43d8-8f68-76f1ca7bfa9b",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "fe932616-8228-491e-8aa0-67ee84b1dd9f", "AQAAAAEAACcQAAAAEBPZHybDlbMA4iZAu1dhOMct7t6zS/+s/U7gM3bKnKSX0xU2C22vvhwJw6qcPUYhEg==" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserName_Gender",
                schema: "User",
                table: "Users",
                columns: new[] { "UserName", "Gender" });

            migrationBuilder.CreateIndex(
                name: "IX_WebPages_Title",
                schema: "System",
                table: "WebPages",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_Entries_TitleShort_TitleLong",
                schema: "Lexicon",
                table: "Entries",
                columns: new[] { "TitleShort", "TitleLong" });

            migrationBuilder.CreateIndex(
                name: "IX_Promotions_Title",
                schema: "Coupon",
                table: "Promotions",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username_Displayname",
                schema: "CMS",
                table: "Users",
                columns: new[] { "Username", "Displayname" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_UserName_Gender",
                schema: "User",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_WebPages_Title",
                schema: "System",
                table: "WebPages");

            migrationBuilder.DropIndex(
                name: "IX_Entries_TitleShort_TitleLong",
                schema: "Lexicon",
                table: "Entries");

            migrationBuilder.DropIndex(
                name: "IX_Promotions_Title",
                schema: "Coupon",
                table: "Promotions");

            migrationBuilder.DropIndex(
                name: "IX_Users_Username_Displayname",
                schema: "CMS",
                table: "Users");

            migrationBuilder.AlterColumn<decimal>(
                name: "Value",
                schema: "User",
                table: "WeightHistory",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(4,1)");

            migrationBuilder.UpdateData(
                schema: "User",
                table: "Users",
                keyColumn: "Id",
                keyValue: "3ee857ac-26ee-43d8-8f68-76f1ca7bfa9b",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "1f05e5d6-8705-4a8e-97e3-43194794bd1f", "AQAAAAEAACcQAAAAEHBJFQ3pRAlCn/ZI3dKj2EOb1djcEuQ+Kpft881OPELxZUErkA6bBlEQAz/vor0Dbg==" });
        }
    }
}
