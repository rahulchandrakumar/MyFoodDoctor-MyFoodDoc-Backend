using Microsoft.EntityFrameworkCore.Migrations;

namespace MyFoodDoc.Database.Migrations
{
    public partial class MethodsAddLinks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DietMethods",
                schema: "System",
                columns: table => new
                {
                    DietId = table.Column<int>(nullable: false),
                    MethodId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DietMethods", x => new { x.DietId, x.MethodId });
                    table.ForeignKey(
                        name: "FK_DietMethods_Diets_DietId",
                        column: x => x.DietId,
                        principalSchema: "System",
                        principalTable: "Diets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DietMethods_Methods_MethodId",
                        column: x => x.MethodId,
                        principalSchema: "System",
                        principalTable: "Methods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IndicationMethods",
                schema: "System",
                columns: table => new
                {
                    IndicationId = table.Column<int>(nullable: false),
                    MethodId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndicationMethods", x => new { x.IndicationId, x.MethodId });
                    table.ForeignKey(
                        name: "FK_IndicationMethods_Indications_IndicationId",
                        column: x => x.IndicationId,
                        principalSchema: "System",
                        principalTable: "Indications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IndicationMethods_Methods_MethodId",
                        column: x => x.MethodId,
                        principalSchema: "System",
                        principalTable: "Methods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MotivationMethods",
                schema: "System",
                columns: table => new
                {
                    MotivationId = table.Column<int>(nullable: false),
                    MethodId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MotivationMethods", x => new { x.MotivationId, x.MethodId });
                    table.ForeignKey(
                        name: "FK_MotivationMethods_Methods_MethodId",
                        column: x => x.MethodId,
                        principalSchema: "System",
                        principalTable: "Methods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MotivationMethods_Motivations_MotivationId",
                        column: x => x.MotivationId,
                        principalSchema: "System",
                        principalTable: "Motivations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DietMethods_MethodId",
                schema: "System",
                table: "DietMethods",
                column: "MethodId");

            migrationBuilder.CreateIndex(
                name: "IX_IndicationMethods_MethodId",
                schema: "System",
                table: "IndicationMethods",
                column: "MethodId");

            migrationBuilder.CreateIndex(
                name: "IX_MotivationMethods_MethodId",
                schema: "System",
                table: "MotivationMethods",
                column: "MethodId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DietMethods",
                schema: "System");

            migrationBuilder.DropTable(
                name: "IndicationMethods",
                schema: "System");

            migrationBuilder.DropTable(
                name: "MotivationMethods",
                schema: "System");
        }
    }
}
