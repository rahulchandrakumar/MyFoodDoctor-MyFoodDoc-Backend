using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace MyFoodDoc.Database.Migrations
{
    public partial class RemoveTestData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Lexicon",
                table: "Entries",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "Lexicon",
                table: "Entries",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "System",
                table: "Images",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "User",
                table: "Users",
                keyColumn: "Id",
                keyValue: "3ee857ac-26ee-43d8-8f68-76f1ca7bfa9b");

            migrationBuilder.DeleteData(
                schema: "System",
                table: "Images",
                keyColumn: "Id",
                keyValue: 1);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.InsertData(
                schema: "User",
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "Birthday", "ConcurrencyStamp", "Created", "Email", "EmailConfirmed", "Gender", "Height", "InsuranceId", "LastModified", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "3ee857ac-26ee-43d8-8f68-76f1ca7bfa9b", 0, null, "95cfd85c-2c9d-401f-966c-e00773f0909b", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "test@appsfactory.de", true, null, null, 1, null, false, null, "TEST@APPSFACTORY.DE", "TEST@APPSFACTORY.DE", "AQAAAAEAACcQAAAAEHlWYYtSCDGesUran3d0MjCCjCxpROjXC0yPOKVRe5jI+p+C6NyiNpjAyRUkGHcquw==", null, false, "", false, "test@appsfactory.de" });

            migrationBuilder.InsertData(
                schema: "Lexicon",
                table: "Entries",
                columns: new[] { "Id", "Created", "ImageId", "LastModified", "Text", "TitleLong", "TitleShort" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, null, "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet.", "Eiweiß", "Eiweiß" });

            migrationBuilder.InsertData(
                schema: "Lexicon",
                table: "Entries",
                columns: new[] { "Id", "Created", "ImageId", "LastModified", "Text", "TitleLong", "TitleShort" },
                values: new object[] { 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, null, "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet.", "Proteine", "Proteine" });
        }
    }
}
