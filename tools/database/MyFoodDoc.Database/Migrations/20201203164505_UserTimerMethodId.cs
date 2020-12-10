using Microsoft.EntityFrameworkCore.Migrations;

namespace MyFoodDoc.Database.Migrations
{
    public partial class UserTimerMethodId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MethodId",
                schema: "System",
                table: "UserTimer",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserTimer_MethodId",
                schema: "System",
                table: "UserTimer",
                column: "MethodId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserTimer_Methods_MethodId",
                schema: "System",
                table: "UserTimer",
                column: "MethodId",
                principalSchema: "System",
                principalTable: "Methods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserTimer_Methods_MethodId",
                schema: "System",
                table: "UserTimer");

            migrationBuilder.DropIndex(
                name: "IX_UserTimer_MethodId",
                schema: "System",
                table: "UserTimer");

            migrationBuilder.DropColumn(
                name: "MethodId",
                schema: "System",
                table: "UserTimer");
        }
    }
}
