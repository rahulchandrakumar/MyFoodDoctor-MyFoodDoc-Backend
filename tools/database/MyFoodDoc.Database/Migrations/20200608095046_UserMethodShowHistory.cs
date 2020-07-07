using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyFoodDoc.Database.Migrations
{
    public partial class UserMethodShowHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TargetAnswerCode",
                schema: "System",
                table: "UserTargets",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "UserMethodShowHistory",
                schema: "System",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(nullable: false, defaultValueSql: "getutcdate()"),
                    UserId = table.Column<string>(maxLength: 450, nullable: false),
                    MethodId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMethodShowHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserMethodShowHistory_Methods_MethodId",
                        column: x => x.MethodId,
                        principalSchema: "System",
                        principalTable: "Methods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserMethodShowHistory_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "User",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserMethodShowHistory_MethodId",
                schema: "System",
                table: "UserMethodShowHistory",
                column: "MethodId");

            migrationBuilder.CreateIndex(
                name: "IX_UserMethodShowHistory_UserId",
                schema: "System",
                table: "UserMethodShowHistory",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserMethodShowHistory",
                schema: "System");

            migrationBuilder.AlterColumn<string>(
                name: "TargetAnswerCode",
                schema: "System",
                table: "UserTargets",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
