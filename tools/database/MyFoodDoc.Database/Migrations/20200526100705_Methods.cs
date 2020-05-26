using System;
using Microsoft.EntityFrameworkCore.Migrations;

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
                    Question = table.Column<string>(maxLength: 1000, nullable: false),
                    Answer = table.Column<bool>(nullable: false)
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
                    Answer = table.Column<bool>(nullable: false),
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
                        name: "FK_UserMethods_MethodMultipleChoice_MethodMultipleChoiceId",
                        column: x => x.MethodMultipleChoiceId,
                        principalSchema: "System",
                        principalTable: "MethodMultipleChoice",
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
                name: "IX_UserMethods_MethodMultipleChoiceId",
                schema: "System",
                table: "UserMethods",
                column: "MethodMultipleChoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_UserMethods_UserId",
                schema: "System",
                table: "UserMethods",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserMethods",
                schema: "System");

            migrationBuilder.DropTable(
                name: "MethodMultipleChoice",
                schema: "System");

            migrationBuilder.DropTable(
                name: "Methods",
                schema: "System");
        }
    }
}
