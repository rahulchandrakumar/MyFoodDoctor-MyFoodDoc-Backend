using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyFoodDoc.Database.Migrations
{
    public partial class Psychogramm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Psychogramm");

            migrationBuilder.CreateTable(
                name: "Scales",
                schema: "Psychogramm",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Text = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    ImageId = table.Column<int>(type: "int", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Scales_Images_ImageId",
                        column: x => x.ImageId,
                        principalSchema: "System",
                        principalTable: "Images",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                schema: "Psychogramm",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    VerticalAlignment = table.Column<bool>(type: "bit", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    ScaleId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Questions_Scales_ScaleId",
                        column: x => x.ScaleId,
                        principalSchema: "Psychogramm",
                        principalTable: "Scales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Choices",
                schema: "Psychogramm",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Scorable = table.Column<bool>(type: "bit", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Choices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Choices_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalSchema: "Psychogramm",
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserChoices",
                schema: "Psychogramm",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    ChoiceId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserChoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserChoices_Choices_ChoiceId",
                        column: x => x.ChoiceId,
                        principalSchema: "Psychogramm",
                        principalTable: "Choices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserChoices_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "User",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Choices_QuestionId",
                schema: "Psychogramm",
                table: "Choices",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_ScaleId",
                schema: "Psychogramm",
                table: "Questions",
                column: "ScaleId");

            migrationBuilder.CreateIndex(
                name: "IX_Scales_ImageId",
                schema: "Psychogramm",
                table: "Scales",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_UserChoices_ChoiceId",
                schema: "Psychogramm",
                table: "UserChoices",
                column: "ChoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_UserChoices_UserId",
                schema: "Psychogramm",
                table: "UserChoices",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserChoices",
                schema: "Psychogramm");

            migrationBuilder.DropTable(
                name: "Choices",
                schema: "Psychogramm");

            migrationBuilder.DropTable(
                name: "Questions",
                schema: "Psychogramm");

            migrationBuilder.DropTable(
                name: "Scales",
                schema: "Psychogramm");
        }
    }
}
