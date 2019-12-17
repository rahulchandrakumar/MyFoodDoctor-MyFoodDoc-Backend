using Microsoft.EntityFrameworkCore.Migrations;

namespace MyFoodDoc.Database.Migrations
{
    public partial class DecimalValues : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Height",
                schema: "User",
                table: "Users",
                type: "decimal(4,1)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Value",
                schema: "User",
                table: "AbdonimalGirthHistory",
                type: "decimal(4,1)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                schema: "User",
                table: "Users",
                keyColumn: "Id",
                keyValue: "3ee857ac-26ee-43d8-8f68-76f1ca7bfa9b",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "95cfd85c-2c9d-401f-966c-e00773f0909b", "AQAAAAEAACcQAAAAEHlWYYtSCDGesUran3d0MjCCjCxpROjXC0yPOKVRe5jI+p+C6NyiNpjAyRUkGHcquw==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Height",
                schema: "User",
                table: "Users",
                type: "int",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(4,1)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Value",
                schema: "User",
                table: "AbdonimalGirthHistory",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(4,1)");

            migrationBuilder.UpdateData(
                schema: "User",
                table: "Users",
                keyColumn: "Id",
                keyValue: "3ee857ac-26ee-43d8-8f68-76f1ca7bfa9b",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "fe932616-8228-491e-8aa0-67ee84b1dd9f", "AQAAAAEAACcQAAAAEBPZHybDlbMA4iZAu1dhOMct7t6zS/+s/U7gM3bKnKSX0xU2C22vvhwJw6qcPUYhEg==" });
        }
    }
}
