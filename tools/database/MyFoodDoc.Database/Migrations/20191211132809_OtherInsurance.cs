using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyFoodDoc.Database.Migrations
{
    public partial class OtherInsurance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "User",
                table: "Users",
                keyColumn: "Id",
                keyValue: "3ee857ac-26ee-43d8-8f68-76f1ca7bfa9b",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "3b7b82e6-5358-4203-b217-973526290323", "AQAAAAEAACcQAAAAEKgPwUxz4n+40tA8cmrTQ+KYmg7ibOggiOl5O0FLOQ+9+dURQ3gQNYPRfpIjgHkheg==" });

            migrationBuilder.InsertData(
                schema: "Values",
                table: "Insurances",
                columns: new[] { "Id", "Created", "LastModified", "Name" },
                values: new object[] { -1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Other" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Values",
                table: "Insurances",
                keyColumn: "Id",
                keyValue: -1);

            migrationBuilder.UpdateData(
                schema: "User",
                table: "Users",
                keyColumn: "Id",
                keyValue: "3ee857ac-26ee-43d8-8f68-76f1ca7bfa9b",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "a1b19c0a-42a3-4050-a7f7-391ec0e0e760", "AQAAAAEAACcQAAAAEBYzVKiS6N4MKsUx05LAiKSLklmMvK3m2Pj8jlNnx5XrHHho+hTazqWbfS2LJxTKAg==" });
        }
    }
}
