using Microsoft.EntityFrameworkCore.Migrations;

namespace MyFoodDoc.Database.Migrations
{
    public partial class TargetMethods : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TargetMethods",
                schema: "System",
                columns: table => new
                {
                    TargetId = table.Column<int>(nullable: false),
                    MethodId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TargetMethods", x => new { x.TargetId, x.MethodId });
                    table.ForeignKey(
                        name: "FK_TargetMethods_Methods_MethodId",
                        column: x => x.MethodId,
                        principalSchema: "System",
                        principalTable: "Methods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TargetMethods_Targets_TargetId",
                        column: x => x.TargetId,
                        principalSchema: "System",
                        principalTable: "Targets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TargetMethods_MethodId",
                schema: "System",
                table: "TargetMethods",
                column: "MethodId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TargetMethods",
                schema: "System");
        }
    }
}
