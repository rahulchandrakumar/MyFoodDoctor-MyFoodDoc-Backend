using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace MyFoodDoc.Database.Migrations
{
    public partial class LinkingImages : Migration
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

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                schema: "Lexicon",
                table: "Entries");

            migrationBuilder.AddColumn<int>(
                name: "ImageId",
                schema: "Lexicon",
                table: "Entries",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                schema: "System",
                table: "Images",
                columns: new[] { "Id", "Created", "LastModified", "Url" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "https://myfooddoctormockcmsimgs.blob.core.windows.net/images/253f35f4-f3ac-425c-93ff-6edfdb62a12f.jpg" });

            migrationBuilder.InsertData(
                schema: "System",
                table: "Images",
                columns: new[] { "Id", "Created", "LastModified", "Url" },
                values: new object[] { 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "https://myfooddoctormockcmsimgs.blob.core.windows.net/images/a5ec0b6f-d3cc-4a52-8283-2d7dba9a560c.jpg" });

            migrationBuilder.UpdateData(
                schema: "User",
                table: "Users",
                keyColumn: "Id",
                keyValue: "3ee857ac-26ee-43d8-8f68-76f1ca7bfa9b",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "33b4c6e4-83e2-4495-a1ee-1e7beed80393", "AQAAAAEAACcQAAAAEJoymUxzrJPUhrXpFBDPPG+lee4Uwp8OcUpB2AbhFhsfm8cSmCc7tlAXRiyL8mqLwg==" });

            migrationBuilder.UpdateData(
                schema: "Lexicon",
                table: "Entries",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImageId",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "Lexicon",
                table: "Entries",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImageId",
                value: 1);

            migrationBuilder.CreateIndex(
                name: "IX_Entries_ImageId",
                schema: "Lexicon",
                table: "Entries",
                column: "ImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Entries_Images_ImageId",
                schema: "Lexicon",
                table: "Entries",
                column: "ImageId",
                principalSchema: "System",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
                name: "FK_Entries_Images_ImageId",
                schema: "Lexicon",
                table: "Entries");

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

            migrationBuilder.DropIndex(
                name: "IX_Entries_ImageId",
                schema: "Lexicon",
                table: "Entries");

            migrationBuilder.DeleteData(
                schema: "System",
                table: "Images",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "System",
                table: "Images",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DropColumn(
                name: "ImageId",
                schema: "Lexicon",
                table: "Entries");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                schema: "Lexicon",
                table: "Entries",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.UpdateData(
                schema: "Lexicon",
                table: "Entries",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImageUrl",
                value: "");

            migrationBuilder.UpdateData(
                schema: "Lexicon",
                table: "Entries",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImageUrl",
                value: "");

            migrationBuilder.UpdateData(
                schema: "User",
                table: "Users",
                keyColumn: "Id",
                keyValue: "3ee857ac-26ee-43d8-8f68-76f1ca7bfa9b",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "984d76b8-a7a3-4b51-aae1-2142b785dbfb", "AQAAAAEAACcQAAAAEJ6ydXqrzc+kxqUY0ztlAgyB0ADFLmvWlUSEZQadER6SwrMaW8sCyL0Sky81GoPKeg==" });

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
