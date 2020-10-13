using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyFoodDoc.Database.Migrations
{
    public partial class LexiconCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entries_Images_ImageId",
                schema: "Lexicon",
                table: "Entries");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                schema: "Lexicon",
                table: "Entries",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Categories",
                schema: "Lexicon",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true),
                    Title = table.Column<string>(maxLength: 100, nullable: false),
                    ImageId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categories_Images_ImageId",
                        column: x => x.ImageId,
                        principalSchema: "System",
                        principalTable: "Images",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Entries_CategoryId",
                schema: "Lexicon",
                table: "Entries",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ImageId",
                schema: "Lexicon",
                table: "Categories",
                column: "ImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Entries_Categories_CategoryId",
                schema: "Lexicon",
                table: "Entries",
                column: "CategoryId",
                principalSchema: "Lexicon",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Entries_Images_ImageId",
                schema: "Lexicon",
                table: "Entries",
                column: "ImageId",
                principalSchema: "System",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entries_Categories_CategoryId",
                schema: "Lexicon",
                table: "Entries");

            migrationBuilder.DropForeignKey(
                name: "FK_Entries_Images_ImageId",
                schema: "Lexicon",
                table: "Entries");

            migrationBuilder.DropTable(
                name: "Categories",
                schema: "Lexicon");

            migrationBuilder.DropIndex(
                name: "IX_Entries_CategoryId",
                schema: "Lexicon",
                table: "Entries");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                schema: "Lexicon",
                table: "Entries");

            migrationBuilder.AddForeignKey(
                name: "FK_Entries_Images_ImageId",
                schema: "Lexicon",
                table: "Entries",
                column: "ImageId",
                principalSchema: "System",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
