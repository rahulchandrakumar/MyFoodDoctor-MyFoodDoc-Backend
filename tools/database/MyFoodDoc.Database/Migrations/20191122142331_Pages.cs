using Microsoft.EntityFrameworkCore.Migrations;

namespace MyFoodDoc.Database.Migrations
{
    public partial class Pages : Migration
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

            migrationBuilder.CreateTable(
                name: "WebPages",
                schema: "System",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(maxLength: 2083, nullable: false),
                    Title = table.Column<string>(maxLength: 200, nullable: false),
                    Text = table.Column<string>(nullable: false),
                    IsDeletable = table.Column<bool>(nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebPages", x => x.Id);
                });

            migrationBuilder.UpdateData(
                schema: "User",
                table: "Users",
                keyColumn: "Id",
                keyValue: "3ee857ac-26ee-43d8-8f68-76f1ca7bfa9b",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "f061fe53-5d7b-4acf-bf1f-463163955e44", "AQAAAAEAACcQAAAAEETfwxEVWP1h+OoQDFO+//gt3enaJTqNauZkcI1tA80hRkITjcEYzgxrWwR8ZwiUuQ==" });

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
                name: "WebPages",
                schema: "System");

            migrationBuilder.UpdateData(
                schema: "User",
                table: "Users",
                keyColumn: "Id",
                keyValue: "3ee857ac-26ee-43d8-8f68-76f1ca7bfa9b",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "33b4c6e4-83e2-4495-a1ee-1e7beed80393", "AQAAAAEAACcQAAAAEJoymUxzrJPUhrXpFBDPPG+lee4Uwp8OcUpB2AbhFhsfm8cSmCc7tlAXRiyL8mqLwg==" });

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
