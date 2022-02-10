using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace MyFoodDoc.Database.Migrations
{
    public partial class Methods : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Methods",
                schema: "System",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true),
                    TargetId = table.Column<int>(nullable: false),
                    Type = table.Column<string>(maxLength: 14, nullable: false),
                    Title = table.Column<string>(maxLength: 100, nullable: false),
                    Text = table.Column<string>(maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Methods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Methods_Targets_TargetId",
                        column: x => x.TargetId,
                        principalSchema: "System",
                        principalTable: "Targets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MethodMultipleChoice",
                schema: "System",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true),
                    MethodId = table.Column<int>(nullable: false),
                    Title = table.Column<string>(maxLength: 100, nullable: false),
                    IsCorrect = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MethodMultipleChoice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MethodMultipleChoice_Methods_MethodId",
                        column: x => x.MethodId,
                        principalSchema: "System",
                        principalTable: "Methods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserMethods",
                schema: "System",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true),
                    UserId = table.Column<string>(maxLength: 450, nullable: false),
                    MethodId = table.Column<int>(nullable: false),
                    Answer = table.Column<bool>(nullable: true),
                    MethodMultipleChoiceId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMethods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserMethods_Methods_MethodId",
                        column: x => x.MethodId,
                        principalSchema: "System",
                        principalTable: "Methods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserMethods_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "User",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                schema: "System",
                table: "OptimizationAreas",
                keyColumn: "Id",
                keyValue: 1,
                column: "Text",
                value: @"Gemüse und Salat haben einen hohen Wasseranteil, wodurch sie sehr kalorienarm sind. Die enthaltenen Ballaststoffe quellen im Magen-Darm-Trakt auf, wodurch du lange satt bleibst.
Die Ballaststoffe aus Gemüse und Salat sorgen für eine gute Verdauung, wodurch dein Darm gesund bleibt.
Gemüse und Salat sind reich an Vitaminen und sekundären Pflanzenstoffen. Sie sorgen für ein starkes Immunsystem.");

            migrationBuilder.UpdateData(
                schema: "System",
                table: "OptimizationAreas",
                keyColumn: "Id",
                keyValue: 2,
                column: "Text",
                value: @"Zuckerreiche Lebensmittel liefern schnell, aber nur kurzfristige Energie. Es kommt zu einem sehr schnellen Anstieg des Blutzuckers, was zu einer sehr schnellen und starken Insulinausschüttung aus der Bauchspeicheldrüse führt.
Dieser Mechanismus fördert Heißhungerattacken und Übergewicht.");

            migrationBuilder.UpdateData(
                schema: "System",
                table: "OptimizationAreas",
                keyColumn: "Id",
                keyValue: 3,
                column: "Text",
                value: @"Eiweiß ist ein lebensnotweniger Nährstoff. Es sorgt für den Erhalt und den Aufbau unserer Muskulatur und unterstützt unser Immunsystem.
Zusätzlich sorgt eine eiweißreiche Mahlzeit für weniger Blutzuckerschwankungen und eine lang anhaltende Sättigung.");

            migrationBuilder.CreateIndex(
                name: "IX_MethodMultipleChoice_MethodId",
                schema: "System",
                table: "MethodMultipleChoice",
                column: "MethodId");

            migrationBuilder.CreateIndex(
                name: "IX_Methods_TargetId",
                schema: "System",
                table: "Methods",
                column: "TargetId");

            migrationBuilder.CreateIndex(
                name: "IX_UserMethods_MethodId",
                schema: "System",
                table: "UserMethods",
                column: "MethodId");

            migrationBuilder.CreateIndex(
                name: "IX_UserMethods_UserId",
                schema: "System",
                table: "UserMethods",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MethodMultipleChoice",
                schema: "System");

            migrationBuilder.DropTable(
                name: "UserMethods",
                schema: "System");

            migrationBuilder.DropTable(
                name: "Methods",
                schema: "System");

            migrationBuilder.UpdateData(
                schema: "System",
                table: "OptimizationAreas",
                keyColumn: "Id",
                keyValue: 1,
                column: "Text",
                value: @"Gemüse und Salat haben einen hohen Wasseranteil, wodurch sie sehr kalorienarm sind. Die enthaltenen Ballaststoffe quellen im Magen-Darm-Trakt auf, wodurch du lange satt bleibst.
Die Ballaststoffe aus Gemüse und Salat sorgen für eine gute Verdauung, wodurch dein Darm gesund bleibt.
Gemüse und Salat sind reich an Vitaminen und sekundären Pflanzenstoffen. Sie sorgen für ein starkes Immunsystem.");

            migrationBuilder.UpdateData(
                schema: "System",
                table: "OptimizationAreas",
                keyColumn: "Id",
                keyValue: 2,
                column: "Text",
                value: @"Zuckerreiche Lebensmittel liefern schnell, aber nur kurzfristige Energie. Es kommt zu einem sehr schnellen Anstieg des Blutzuckers, was zu einer sehr schnellen und starken Insulinausschüttung aus der Bauchspeicheldrüse führt.
Dieser Mechanismus fördert Heißhungerattacken und Übergewicht.");

            migrationBuilder.UpdateData(
                schema: "System",
                table: "OptimizationAreas",
                keyColumn: "Id",
                keyValue: 3,
                column: "Text",
                value: @"Eiweiß ist ein lebensnotweniger Nährstoff. Es sorgt für den Erhalt und den Aufbau unserer Muskulatur und unterstützt unser Immunsystem.
Zusätzlich sorgt eine eiweißreiche Mahlzeit für weniger Blutzuckerschwankungen und eine lang anhaltende Sättigung.");
        }
    }
}
