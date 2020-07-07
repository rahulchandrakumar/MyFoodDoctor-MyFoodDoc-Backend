using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyFoodDoc.Database.Migrations
{
    public partial class Targets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OptimizationAreas",
                schema: "System",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true),
                    Key = table.Column<string>(maxLength: 50, nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Text = table.Column<string>(maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OptimizationAreas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Targets",
                schema: "System",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true),
                    OptimizationAreaId = table.Column<int>(nullable: false),
                    TriggerOperator = table.Column<string>(maxLength: 11, nullable: false),
                    TriggerValue = table.Column<int>(nullable: false),
                    Threshold = table.Column<int>(nullable: false),
                    Priority = table.Column<string>(maxLength: 6, nullable: false),
                    Title = table.Column<string>(maxLength: 1000, nullable: false),
                    Text = table.Column<string>(maxLength: 1000, nullable: false),
                    Type = table.Column<string>(maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Targets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Targets_OptimizationAreas_OptimizationAreaId",
                        column: x => x.OptimizationAreaId,
                        principalSchema: "System",
                        principalTable: "OptimizationAreas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AdjustmentTargets",
                schema: "System",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true),
                    TargetId = table.Column<int>(nullable: false),
                    TargetValue = table.Column<int>(nullable: false),
                    Step = table.Column<int>(nullable: false),
                    StepDirection = table.Column<string>(maxLength: 10, nullable: false),
                    RecommendedText = table.Column<string>(maxLength: 1000, nullable: false),
                    TargetText = table.Column<string>(maxLength: 1000, nullable: false),
                    RemainText = table.Column<string>(maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdjustmentTargets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdjustmentTargets_Targets_TargetId",
                        column: x => x.TargetId,
                        principalSchema: "System",
                        principalTable: "Targets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DietTargets",
                schema: "System",
                columns: table => new
                {
                    DietId = table.Column<int>(nullable: false),
                    TargetId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DietTargets", x => new { x.DietId, x.TargetId });
                    table.ForeignKey(
                        name: "FK_DietTargets_Diets_DietId",
                        column: x => x.DietId,
                        principalSchema: "System",
                        principalTable: "Diets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DietTargets_Targets_TargetId",
                        column: x => x.TargetId,
                        principalSchema: "System",
                        principalTable: "Targets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IndicationTargets",
                schema: "System",
                columns: table => new
                {
                    IndicationId = table.Column<int>(nullable: false),
                    TargetId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndicationTargets", x => new { x.IndicationId, x.TargetId });
                    table.ForeignKey(
                        name: "FK_IndicationTargets_Indications_IndicationId",
                        column: x => x.IndicationId,
                        principalSchema: "System",
                        principalTable: "Indications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IndicationTargets_Targets_TargetId",
                        column: x => x.TargetId,
                        principalSchema: "System",
                        principalTable: "Targets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MotivationTargets",
                schema: "System",
                columns: table => new
                {
                    MotivationId = table.Column<int>(nullable: false),
                    TargetId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MotivationTargets", x => new { x.MotivationId, x.TargetId });
                    table.ForeignKey(
                        name: "FK_MotivationTargets_Motivations_MotivationId",
                        column: x => x.MotivationId,
                        principalSchema: "System",
                        principalTable: "Motivations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MotivationTargets_Targets_TargetId",
                        column: x => x.TargetId,
                        principalSchema: "System",
                        principalTable: "Targets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTargets",
                schema: "System",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    TargetId = table.Column<int>(nullable: false),
                    TargetAnswerCode = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTargets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserTargets_Targets_TargetId",
                        column: x => x.TargetId,
                        principalSchema: "System",
                        principalTable: "Targets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserTargets_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "User",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "System",
                table: "OptimizationAreas",
                columns: new[] { "Id", "Created", "Key", "LastModified", "Name", "Text" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vegetables", null, "Gemüse", @"Gemüse und Salat haben einen hohen Wasseranteil, wodurch sie sehr kalorienarm sind. Die enthaltenen Ballaststoffe quellen im Magen-Darm-Trakt auf, wodurch du lange satt bleibst.
Die Ballaststoffe aus Gemüse und Salat sorgen für eine gute Verdauung, wodurch dein Darm gesund bleibt.
Gemüse und Salat sind reich an Vitaminen und sekundären Pflanzenstoffen. Sie sorgen für ein starkes Immunsystem." });

            migrationBuilder.InsertData(
                schema: "System",
                table: "OptimizationAreas",
                columns: new[] { "Id", "Created", "Key", "LastModified", "Name", "Text" },
                values: new object[] { 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "sugar", null, "Zucker", @"Zuckerreiche Lebensmittel liefern schnell, aber nur kurzfristige Energie. Es kommt zu einem sehr schnellen Anstieg des Blutzuckers, was zu einer sehr schnellen und starken Insulinausschüttung aus der Bauchspeicheldrüse führt.
Dieser Mechanismus fördert Heißhungerattacken und Übergewicht." });

            migrationBuilder.InsertData(
                schema: "System",
                table: "OptimizationAreas",
                columns: new[] { "Id", "Created", "Key", "LastModified", "Name", "Text" },
                values: new object[] { 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "protein", null, "Proteine", @"Eiweiß ist ein lebensnotweniger Nährstoff. Es sorgt für den Erhalt und den Aufbau unserer Muskulatur und unterstützt unser Immunsystem.
Zusätzlich sorgt eine eiweißreiche Mahlzeit für weniger Blutzuckerschwankungen und eine lang anhaltende Sättigung." });

            migrationBuilder.CreateIndex(
                name: "IX_AdjustmentTargets_TargetId",
                schema: "System",
                table: "AdjustmentTargets",
                column: "TargetId");

            migrationBuilder.CreateIndex(
                name: "IX_DietTargets_TargetId",
                schema: "System",
                table: "DietTargets",
                column: "TargetId");

            migrationBuilder.CreateIndex(
                name: "IX_IndicationTargets_TargetId",
                schema: "System",
                table: "IndicationTargets",
                column: "TargetId");

            migrationBuilder.CreateIndex(
                name: "IX_MotivationTargets_TargetId",
                schema: "System",
                table: "MotivationTargets",
                column: "TargetId");

            migrationBuilder.CreateIndex(
                name: "IX_OptimizationAreas_Key",
                schema: "System",
                table: "OptimizationAreas",
                column: "Key");

            migrationBuilder.CreateIndex(
                name: "IX_Targets_OptimizationAreaId",
                schema: "System",
                table: "Targets",
                column: "OptimizationAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTargets_TargetId",
                schema: "System",
                table: "UserTargets",
                column: "TargetId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTargets_UserId",
                schema: "System",
                table: "UserTargets",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdjustmentTargets",
                schema: "System");

            migrationBuilder.DropTable(
                name: "DietTargets",
                schema: "System");

            migrationBuilder.DropTable(
                name: "IndicationTargets",
                schema: "System");

            migrationBuilder.DropTable(
                name: "MotivationTargets",
                schema: "System");

            migrationBuilder.DropTable(
                name: "UserTargets",
                schema: "System");

            migrationBuilder.DropTable(
                name: "Targets",
                schema: "System");

            migrationBuilder.DropTable(
                name: "OptimizationAreas",
                schema: "System");
        }
    }
}
