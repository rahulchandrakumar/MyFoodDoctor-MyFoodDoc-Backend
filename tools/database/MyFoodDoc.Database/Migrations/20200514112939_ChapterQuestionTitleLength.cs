using Microsoft.EntityFrameworkCore.Migrations;

namespace MyFoodDoc.Database.Migrations
{
    public partial class ChapterQuestionTitleLength : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "QuestionTitle",
                schema: "Course",
                table: "Chapters",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.UpdateData(
                schema: "System",
                table: "OptimizationAreas",
                keyColumn: "Id",
                keyValue: 1,
                column: "Text",
                value: @"Gemüse und Salat haben einen hohen Wasseranteil, wodurch sie sehr kalorienarm sind. Die enthaltenen Ballaststoffe quellen im Magen-Darm-Trakt auf, wodurch du lange satt bleibst.
Die Ballaststoffe aus Gemüse und Salat sorgen für eine gute Verdauung, wodurch dein Darm gesund bleibt.
Gemüse und Salat sind reich an Vitaminen und sekundären Pflanzenstoffen. Sie sorgen für ein starkes Immunsystem.");

            migrationBuilder.UpdateData(
                schema: "System",
                table: "OptimizationAreas",
                keyColumn: "Id",
                keyValue: 2,
                column: "Text",
                value: @"Zuckerreiche Lebensmittel liefern schnell, aber nur kurzfristige Energie. Es kommt zu einem sehr schnellen Anstieg des Blutzuckers, was zu einer sehr schnellen und starken Insulinausschüttung aus der Bauchspeicheldrüse führt.
Dieser Mechanismus fördert Heißhungerattacken und Übergewicht.");

            migrationBuilder.UpdateData(
                schema: "System",
                table: "OptimizationAreas",
                keyColumn: "Id",
                keyValue: 3,
                column: "Text",
                value: @"Eiweiß ist ein lebensnotweniger Nährstoff. Es sorgt für den Erhalt und den Aufbau unserer Muskulatur und unterstützt unser Immunsystem.
Zusätzlich sorgt eine eiweißreiche Mahlzeit für weniger Blutzuckerschwankungen und eine lang anhaltende Sättigung.");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "QuestionTitle",
                schema: "Course",
                table: "Chapters",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 100);

            migrationBuilder.UpdateData(
                schema: "System",
                table: "OptimizationAreas",
                keyColumn: "Id",
                keyValue: 1,
                column: "Text",
                value: @"Gemüse und Salat haben einen hohen Wasseranteil, wodurch sie sehr kalorienarm sind. Die enthaltenen Ballaststoffe quellen im Magen-Darm-Trakt auf, wodurch du lange satt bleibst.
Die Ballaststoffe aus Gemüse und Salat sorgen für eine gute Verdauung, wodurch dein Darm gesund bleibt.
Gemüse und Salat sind reich an Vitaminen und sekundären Pflanzenstoffen. Sie sorgen für ein starkes Immunsystem.");

            migrationBuilder.UpdateData(
                schema: "System",
                table: "OptimizationAreas",
                keyColumn: "Id",
                keyValue: 2,
                column: "Text",
                value: @"Zuckerreiche Lebensmittel liefern schnell, aber nur kurzfristige Energie. Es kommt zu einem sehr schnellen Anstieg des Blutzuckers, was zu einer sehr schnellen und starken Insulinausschüttung aus der Bauchspeicheldrüse führt.
Dieser Mechanismus fördert Heißhungerattacken und Übergewicht.");

            migrationBuilder.UpdateData(
                schema: "System",
                table: "OptimizationAreas",
                keyColumn: "Id",
                keyValue: 3,
                column: "Text",
                value: @"Eiweiß ist ein lebensnotweniger Nährstoff. Es sorgt für den Erhalt und den Aufbau unserer Muskulatur und unterstützt unser Immunsystem.
Zusätzlich sorgt eine eiweißreiche Mahlzeit für weniger Blutzuckerschwankungen und eine lang anhaltende Sättigung.");
        }
    }
}
