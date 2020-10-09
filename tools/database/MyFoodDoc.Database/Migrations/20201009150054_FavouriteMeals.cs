using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyFoodDoc.Database.Migrations
{
    public partial class FavouriteMeals : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Favourites",
                schema: "Diary",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true),
                    UserId = table.Column<string>(maxLength: 450, nullable: false),
                    Title = table.Column<string>(maxLength: 100, nullable: false),
                    IsGeneric = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favourites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Favourites_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "User",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FavouritesIngredients",
                schema: "Diary",
                columns: table => new
                {
                    FavouriteId = table.Column<int>(nullable: false),
                    IngredientId = table.Column<int>(nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavouritesIngredients", x => new { x.FavouriteId, x.IngredientId });
                    table.ForeignKey(
                        name: "FK_FavouritesIngredients_Favourites_FavouriteId",
                        column: x => x.FavouriteId,
                        principalSchema: "Diary",
                        principalTable: "Favourites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FavouritesIngredients_Ingredients_IngredientId",
                        column: x => x.IngredientId,
                        principalSchema: "Food",
                        principalTable: "Ingredients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MealsFavourites",
                schema: "Diary",
                columns: table => new
                {
                    MealId = table.Column<int>(nullable: false),
                    FavouriteId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealsFavourites", x => new { x.MealId, x.FavouriteId });
                    table.ForeignKey(
                        name: "FK_MealsFavourites_Favourites_FavouriteId",
                        column: x => x.FavouriteId,
                        principalSchema: "Diary",
                        principalTable: "Favourites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MealsFavourites_Meals_MealId",
                        column: x => x.MealId,
                        principalSchema: "Diary",
                        principalTable: "Meals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Favourites_UserId",
                schema: "Diary",
                table: "Favourites",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FavouritesIngredients_IngredientId",
                schema: "Diary",
                table: "FavouritesIngredients",
                column: "IngredientId");

            migrationBuilder.CreateIndex(
                name: "IX_MealsFavourites_FavouriteId",
                schema: "Diary",
                table: "MealsFavourites",
                column: "FavouriteId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FavouritesIngredients",
                schema: "Diary");

            migrationBuilder.DropTable(
                name: "MealsFavourites",
                schema: "Diary");

            migrationBuilder.DropTable(
                name: "Favourites",
                schema: "Diary");
        }
    }
}
