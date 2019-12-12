using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyFoodDoc.Database.Migrations
{
    public partial class UserHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BloodSugarLevelHistory",
                schema: "User");

            migrationBuilder.AlterColumn<decimal>(
                name: "Value",
                schema: "User",
                table: "WeightHistory",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                schema: "User",
                table: "Users",
                keyColumn: "Id",
                keyValue: "3ee857ac-26ee-43d8-8f68-76f1ca7bfa9b",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "1f05e5d6-8705-4a8e-97e3-43194794bd1f", "AQAAAAEAACcQAAAAEHBJFQ3pRAlCn/ZI3dKj2EOb1djcEuQ+Kpft881OPELxZUErkA6bBlEQAz/vor0Dbg==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Value",
                schema: "User",
                table: "WeightHistory",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.CreateTable(
                name: "BloodSugarLevelHistory",
                schema: "User",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Date = table.Column<DateTime>(type: "Date", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BloodSugarLevelHistory", x => new { x.UserId, x.Date });
                    table.ForeignKey(
                        name: "FK_BloodSugarLevelHistory_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "User",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                schema: "User",
                table: "Users",
                keyColumn: "Id",
                keyValue: "3ee857ac-26ee-43d8-8f68-76f1ca7bfa9b",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "3b7b82e6-5358-4203-b217-973526290323", "AQAAAAEAACcQAAAAEKgPwUxz4n+40tA8cmrTQ+KYmg7ibOggiOl5O0FLOQ+9+dURQ3gQNYPRfpIjgHkheg==" });
        }
    }
}
