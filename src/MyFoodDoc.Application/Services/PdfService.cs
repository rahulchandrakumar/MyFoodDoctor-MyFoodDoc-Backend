using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Enums;
using Spire.Pdf;
using Spire.Pdf.General.Find;
using Spire.Pdf.Graphics;
using Spire.Pdf.Grid;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace MyFoodDoc.Application.Services
{
    public class PdfService : IPdfService
    {
        private static readonly Regex _precompiledRegex = new Regex(@"^100\s?[gG]", RegexOptions.Compiled);

        private string FormatMeal(DiaryExportMealModel meal)
        {
            static string FormatIngredient(DiaryExportMealIngredientModel x)
            {
                decimal amount = x.Amount;
                string servingDescription = x.ServingDescription;

                if (_precompiledRegex.IsMatch(servingDescription))
                {
                    amount *= 100;

                    return $"{x.FoodName}:\n{amount.ToString("G29")} g";
                }
                else
                {
                    servingDescription = x.MeasurementDescription;
                    int index = x.MeasurementDescription.IndexOf('(');
                    if (index != -1)
                    {
                        servingDescription = servingDescription.Substring(0, index).TrimEnd();
                    }
                    return $"{x.FoodName}:\n{amount.ToString("G29")} x {servingDescription} ({x.MetricServingAmount.ToString("G29")} {x.MetricServingUnit})";
                }
            }

            return string.Join('\n', meal.Ingredients.Select(x => FormatIngredient(x)));
        }

        private float drawTextAndMoveDown(PdfNewPage page, PdfTrueTypeFont font, PdfBrush brush, string s, float x, float y, PdfStringFormat format)
        {
            page.Canvas.DrawString(s, font, brush, x, y, format);

            y += font.MeasureString(s, format).Height;

            return y;
        }

        public byte[] GenerateExport(DiaryExportModel data, Stream logoStream)
        {
            PdfDocument doc = new PdfDocument();

            PdfUnitConvertor unitCvtr = new PdfUnitConvertor();
            PdfMargins margin = new PdfMargins();
            margin.Top = unitCvtr.ConvertUnits(1.5f, PdfGraphicsUnit.Centimeter, PdfGraphicsUnit.Point);
            margin.Bottom = unitCvtr.ConvertUnits(1.0f, PdfGraphicsUnit.Centimeter, PdfGraphicsUnit.Point);
            margin.Left = unitCvtr.ConvertUnits(1.0f, PdfGraphicsUnit.Centimeter, PdfGraphicsUnit.Point);
            margin.Right = unitCvtr.ConvertUnits(1.0f, PdfGraphicsUnit.Centimeter, PdfGraphicsUnit.Point);

            PdfPageSettings ps = new PdfPageSettings(PdfPageSize.A4, PdfPageOrientation.Landscape);
            PdfSection section = doc.Sections.Add(ps);
            section.PageSettings.Margins = margin;
            PdfNewPage page = section.Pages.Add();

            float y = 15;
            float x = 5;

            PdfImage image = PdfImage.FromStream(logoStream);
            page.Canvas.DrawImage(image, 600, 0);

            PdfBrush brush1 = PdfBrushes.Black;
            var font1 = new PdfTrueTypeFont("Helvetica", 16f, PdfFontStyle.Bold);
            var font2 = new PdfTrueTypeFont("Helvetica", 10f, PdfFontStyle.Bold);
            var font3 = new PdfTrueTypeFont("Helvetica", 9f, PdfFontStyle.Regular);
            var font4 = new PdfTrueTypeFont("Helvetica", 8f, PdfFontStyle.Regular);

            PdfStringFormat format1 = new PdfStringFormat(PdfTextAlignment.Left);

            string title = "Ernährungs - Tagebuch";
            y = drawTextAndMoveDown(page, font1, brush1, title, x, y, format1);

            y = drawTextAndMoveDown(page, font2, brush1, "erstellt von der myFoodDoctor App", x, y, format1) + 5;

            y = drawTextAndMoveDown(page, font4, brush1, "Die Mengenangaben in den Klammern beziehen sich immer auf eine Portion.", x, y, format1) + 5;

            page.Canvas.DrawString($"Zeitraum: {data.DateFrom.ToString("dd.MM.yyyy")} - {data.DateTo.ToString("dd.MM.yyyy")}", font4, brush1, 600, y, format1);

            y = drawTextAndMoveDown(page, font4, brush1, "Das Wort Portion steht für verschiedene Maßeinheiten (Tasse, Löffel, etc.)", x, y, format1);
            y = drawTextAndMoveDown(page, font4, brush1, "und kann aus technischen Gründen nur in der Einzahl stehen.", x, y, format1);

            y = y + 10;

            PdfPageTemplateElement footerSpace = new PdfPageTemplateElement(ps.Size.Width, margin.Bottom);

            footerSpace.Foreground = false;

            float x1 = margin.Left;
            float y1 = 5;

            float pageHeight = page.Canvas.ClientSize.Height - margin.Bottom;

            PdfPen pen = new PdfPen(PdfBrushes.Gray, 1);
            footerSpace.Graphics.DrawLine(pen, x1, y1, ps.Size.Width - x1, y1);

            y1 = y1 + 12;
            PdfStringFormat format = new PdfStringFormat(PdfTextAlignment.Center);
            string footerText = "myFoodDoctor GmbH - www.myfooddoctor.de";
            footerSpace.Graphics.DrawString(footerText, font3, PdfBrushes.Gray, page.Canvas.ClientSize.Width / 2, y1, format);

            doc.Template.Bottom = footerSpace;


            PdfGrid grid = new PdfGrid();
            grid.Style.Font = font4;
            grid.Style.CellPadding = new PdfPaddings(4, 4, 4, 4);
            grid.AllowCrossPages = true;

            grid.Columns.Add(10);
            float width = page.Canvas.ClientSize.Width - (grid.Columns.Count + 1);

            grid.Columns[0].Width = width * 0.05f;
            grid.Columns[1].Width = width * 0.05f;
            grid.Columns[2].Width = width * 0.15f;
            grid.Columns[3].Width = width * 0.05f;
            grid.Columns[4].Width = width * 0.15f;
            grid.Columns[5].Width = width * 0.05f;
            grid.Columns[6].Width = width * 0.15f;
            grid.Columns[7].Width = width * 0.05f;
            grid.Columns[8].Width = width * 0.15f;
            grid.Columns[9].Width = width * 0.15f;

            PdfGridRow row0 = grid.Headers.Add(1)[0];

            float height = 25.0f;
            row0.Height = height;

            row0.Cells[0].Value = "Datum";
            row0.Cells[0].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
            row0.Cells[0].Style.Borders.All = new PdfPen(Color.Transparent);

            row0.Cells[1].Value = "Zeit";
            row0.Cells[1].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
            row0.Cells[1].Style.BackgroundBrush = PdfBrushes.LightSteelBlue;
            row0.Cells[1].Style.Borders.All = new PdfPen(Color.Transparent);

            row0.Cells[2].Value = "Frühstück";
            row0.Cells[2].StringFormat = new PdfStringFormat(PdfTextAlignment.Left, PdfVerticalAlignment.Middle);
            row0.Cells[2].Style.BackgroundBrush = PdfBrushes.DarkGray;
            row0.Cells[2].Style.Borders.All = new PdfPen(Color.Transparent);

            row0.Cells[3].Value = "Zeit";
            row0.Cells[3].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
            row0.Cells[3].Style.BackgroundBrush = PdfBrushes.LightSteelBlue;
            row0.Cells[3].Style.Borders.All = new PdfPen(Color.Transparent);

            row0.Cells[4].Value = "Mittagessen";
            row0.Cells[4].StringFormat = new PdfStringFormat(PdfTextAlignment.Left, PdfVerticalAlignment.Middle);
            row0.Cells[4].Style.BackgroundBrush = PdfBrushes.DarkGray;
            row0.Cells[4].Style.Borders.All = new PdfPen(Color.Transparent);

            row0.Cells[5].Value = "Zeit";
            row0.Cells[5].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
            row0.Cells[5].Style.BackgroundBrush = PdfBrushes.LightSteelBlue;
            row0.Cells[5].Style.Borders.All = new PdfPen(Color.Transparent);

            row0.Cells[6].Value = "Abendessen";
            row0.Cells[6].StringFormat = new PdfStringFormat(PdfTextAlignment.Left, PdfVerticalAlignment.Middle);
            row0.Cells[6].Style.BackgroundBrush = PdfBrushes.DarkGray;
            row0.Cells[6].Style.Borders.All = new PdfPen(Color.Transparent);

            row0.Cells[7].Value = "Zeit";
            row0.Cells[7].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
            row0.Cells[7].Style.BackgroundBrush = PdfBrushes.LightSteelBlue;
            row0.Cells[7].Style.Borders.All = new PdfPen(Color.Transparent);

            row0.Cells[8].Value = "Snacks";
            row0.Cells[8].StringFormat = new PdfStringFormat(PdfTextAlignment.Left, PdfVerticalAlignment.Middle);
            row0.Cells[8].Style.BackgroundBrush = PdfBrushes.DarkGray;
            row0.Cells[8].Style.Borders.All = new PdfPen(Color.Transparent);

            row0.Cells[9].Value = "Tageswerte";
            row0.Cells[9].StringFormat = new PdfStringFormat(PdfTextAlignment.Left, PdfVerticalAlignment.Middle);
            row0.Cells[9].Style.BackgroundBrush = PdfBrushes.LightSteelBlue;
            row0.Cells[9].Style.Borders.All = new PdfPen(Color.Transparent);

            grid.RepeatHeader = true;

            float rowY = y + row0.Height;
            PdfGridRow previousRow = null;
            foreach (var day in data.Days)
            {
                PdfGridRow row = grid.Rows.Add();

                row.Cells[0].Value = day.Date.ToString("dd.MM");
                row.Cells[0].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
                row.Cells[0].Style.BackgroundBrush = PdfBrushes.WhiteSmoke;
                row.Cells[0].Style.Borders.All = new PdfPen(Color.White, 5);

                row.Cells[1].ColumnSpan = 2;
                row.Cells[1].Style.BackgroundBrush = PdfBrushes.GhostWhite;
                row.Cells[1].Style.Borders.Top = new PdfPen(Color.White, 5);
                row.Cells[1].Style.Borders.Bottom = new PdfPen(Color.White, 5);
                row.Cells[1].Style.Borders.Left = new PdfPen(Color.White, 5);
                row.Cells[1].Style.Borders.Right = new PdfPen(Color.White, 5);

                row.Cells[3].ColumnSpan = 2;
                row.Cells[3].Style.BackgroundBrush = PdfBrushes.GhostWhite;
                row.Cells[3].Style.Borders.Top = new PdfPen(Color.White, 5);
                row.Cells[3].Style.Borders.Bottom = new PdfPen(Color.White, 5);
                row.Cells[3].Style.Borders.Left = new PdfPen(Color.White, 5);
                row.Cells[3].Style.Borders.Right = new PdfPen(Color.White, 5);

                row.Cells[5].ColumnSpan = 2;
                row.Cells[5].Style.BackgroundBrush = PdfBrushes.GhostWhite;
                row.Cells[5].Style.Borders.Top = new PdfPen(Color.White, 5);
                row.Cells[5].Style.Borders.Bottom = new PdfPen(Color.White, 5);
                row.Cells[5].Style.Borders.Left = new PdfPen(Color.White, 5);
                row.Cells[5].Style.Borders.Right = new PdfPen(Color.White, 5);

                row.Cells[7].ColumnSpan = 2;
                row.Cells[7].Style.BackgroundBrush = PdfBrushes.GhostWhite;
                row.Cells[7].Style.Borders.Top = new PdfPen(Color.White, 5);
                row.Cells[7].Style.Borders.Bottom = new PdfPen(Color.White, 5);
                row.Cells[7].Style.Borders.Left = new PdfPen(Color.White, 5);
                row.Cells[7].Style.Borders.Right = new PdfPen(Color.White, 5);

                foreach (var mealGroup in day.Meals.GroupBy(g => g.Type))
                {
                    PdfGrid embedGrid = new PdfGrid();
                    embedGrid.Style.Font = font4;

                    embedGrid.Columns.Add(2);

                    embedGrid.Columns[0].Width = width * 0.05f;
                    embedGrid.Columns[1].Width = width * 0.15f;

                    foreach (var meal in mealGroup)
                    {
                        PdfGridRow newRow = embedGrid.Rows.Add();

                        newRow.Cells[0].Value = meal.Time.ToString("hh':'mm");
                        newRow.Cells[0].StringFormat = new PdfStringFormat(PdfTextAlignment.Left, PdfVerticalAlignment.Top);
                        newRow.Cells[0].Style.BackgroundBrush = PdfBrushes.GhostWhite;
                        newRow.Cells[0].Style.Borders.All = new PdfPen(Color.Transparent);

                        newRow.Cells[1].Value = FormatMeal(meal);
                        newRow.Cells[1].StringFormat = new PdfStringFormat(PdfTextAlignment.Left, PdfVerticalAlignment.Top);
                        newRow.Cells[1].Style.BackgroundBrush = PdfBrushes.GhostWhite;
                        newRow.Cells[1].Style.Borders.All = new PdfPen(Color.Transparent);
                    }

                    switch (mealGroup.Key)
                    {
                        case MealType.Breakfast:

                            row.Cells[1].Value = embedGrid;

                            break;
                        case MealType.Lunch:

                            row.Cells[3].Value = embedGrid;

                            break;
                        case MealType.Dinner:

                            row.Cells[5].Value = embedGrid;

                            break;
                        case MealType.Snack:

                            row.Cells[7].Value = embedGrid;

                            break;
                    }
                }

                string total = $"Kalorien: {day.Calories.ToString("G29")} kcal\n" +
                                $"Gemüse: {day.Vegetables.ToString("G29")} g\n" +
                                $"Proteine: {day.Protein.ToString("G29")} g\n" +
                                $"Zucker: {day.Sugar.ToString("G29")} g\n" +
                                $"Mahlzeiten: {day.Meals.Count}\n" +
                                $"Flüssigkeit: {day.LiquidAmount.ToString("G29")} ml\n" +
                                $"Bewegungsdauer: {day.ExerciseDuration.ToString("G29")} min";

                row.Cells[9].Value = total;
                row.Cells[9].StringFormat = new PdfStringFormat(PdfTextAlignment.Left, PdfVerticalAlignment.Top);
                row.Cells[9].Style.Borders.All = new PdfPen(Color.White, 5);

                if (row.Height + rowY > pageHeight)
                {
                    if (previousRow != null)
                    {
                        float diff = pageHeight - rowY;
                        if (diff > 0)
                        {
                            previousRow.Height += diff;
                            rowY = row.Height + row0.Height;
                        }
                        else
                        {

                        }
                    }
                }
                else
                {
                    rowY += row.Height;
                }

                previousRow = row;
            }

            grid.Draw(page, new PointF(0, y));

            var memoryStream = new MemoryStream();
            doc.SaveToStream(memoryStream);

            return memoryStream.ToArray();
        }

        public byte[] ReplaceText(byte[] bytes, string oldValue, string newValue)
        {
            if (bytes == null || bytes.Length == 0)
                throw new Exception("File is empty");

            PdfDocument doc = new PdfDocument();
            doc.LoadFromBytes(bytes);

            foreach (PdfPageBase page in doc.Pages)
            {
                PdfTextFindCollection collection = page.FindText(oldValue, TextFindParameter.IgnoreCase);

                //Creates a brush
                PdfBrush brush = new PdfSolidBrush(Color.Black);
                //Defines a font
                var font = new PdfTrueTypeFont("Helvetica", 10f, PdfFontStyle.Regular);

                RectangleF rec;
                foreach (PdfTextFind find in collection.Finds)
                {
                    rec = find.TextBounds.FirstOrDefault();
                    page.Canvas.DrawRectangle(PdfBrushes.White, rec);
                    page.Canvas.DrawString(newValue, font, brush, rec.Location.X, rec.Location.Y);
                }
            }

            var memoryStream = new MemoryStream();
            doc.SaveToStream(memoryStream);

            return memoryStream.ToArray();
        }
    }
}
