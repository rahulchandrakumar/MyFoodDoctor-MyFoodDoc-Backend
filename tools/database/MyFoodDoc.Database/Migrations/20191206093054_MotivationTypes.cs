using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace MyFoodDoc.Database.Migrations
{
    public partial class MotivationTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "System",
                table: "Motivations",
                columns: new[] { "Id", "Created", "Key", "LastModified", "Name" },
                values: new object[,]
                {
                    { 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "healthier_lifestyle", null, "Healthier lifestyle" },
                    { 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "reduce_weight", null, "Reduce weight" },
                    { 4, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "feel_better", null, "Feel better" }
                });

            migrationBuilder.UpdateData(
                schema: "User",
                table: "Users",
                keyColumn: "Id",
                keyValue: "3ee857ac-26ee-43d8-8f68-76f1ca7bfa9b",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "e0977a98-fbe2-4470-9ca6-f6daf8fd8cc6", "AQAAAAEAACcQAAAAED6BmuyxLsuW2u9/jKJQSYt87/f5isLD7Pq+FtZlKRBoPUQm7RekebB5xv/RFx+J6Q==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "System",
                table: "Motivations",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "System",
                table: "Motivations",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                schema: "System",
                table: "Motivations",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.UpdateData(
                schema: "User",
                table: "Users",
                keyColumn: "Id",
                keyValue: "3ee857ac-26ee-43d8-8f68-76f1ca7bfa9b",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "061fce92-6b05-4fcf-9378-4551af35d604", "AQAAAAEAACcQAAAAEKFb1tcxc48UHhaiaD1gdDAP5EqRmnVFkWy1EAp9njVf58I9AYtrSGtr2XirDaQhPA==" });
        }
    }
}
