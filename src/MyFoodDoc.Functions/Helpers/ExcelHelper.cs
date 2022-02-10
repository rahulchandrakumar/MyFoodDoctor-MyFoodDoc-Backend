using MyFoodDoc.Application.Entities;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;

namespace MyFoodDoc.Functions.Helpers
{
    public static class ExcelHelper
    {
        public static byte[] CreateExcelFile(UserStatsDto data, bool includeHeader = false)
        {
            void AddSubscriptionRow(ISheet excelSheet, int index, string caption, SubscriptionStatsDto stats)
            {
                IRow row = excelSheet.CreateRow(index);

                row.CreateCell(0).SetCellValue(caption);
                row.CreateCell(1).SetCellValue(stats.MonthlySubscriptions);
                row.CreateCell(2).SetCellValue(stats.HalfYearlySubscriptions);
                row.CreateCell(3).SetCellValue(stats.YearlySubscriptions);
            }
            void AddCounterRow(ISheet excelSheet, int index, string caption, int count)
            {
                IRow row = excelSheet.CreateRow(index);

                var cell0 = row.CreateCell(0);
                cell0.CellStyle.ShrinkToFit = true;
                cell0.SetCellValue(caption);

                row.CreateCell(1).SetCellValue(count);
            }

            using var memoryStream = new MemoryStream();
            IWorkbook workbook = new XSSFWorkbook();

            var font = workbook.CreateFont();
            font.IsBold = true;

            ISheet excelSheet1 = workbook.CreateSheet("Subscriptions");
            ISheet excelSheet2 = workbook.CreateSheet("Number of active users");

            IRow headerRow = excelSheet1.CreateRow(0);
            var headerType = headerRow.CreateCell(0);
            headerType.SetCellValue("Type");
            headerType.CellStyle = workbook.CreateCellStyle();
            headerType.CellStyle.SetFont(font);

            var headerMonthly = headerRow.CreateCell(1);
            headerMonthly.SetCellValue("Monthly");
            headerMonthly.CellStyle = workbook.CreateCellStyle();
            headerMonthly.CellStyle.SetFont(font);

            var headerSemiannual = headerRow.CreateCell(2);
            headerSemiannual.SetCellValue("Half-yearly");
            headerSemiannual.CellStyle = workbook.CreateCellStyle();
            headerSemiannual.CellStyle.SetFont(font);

            var headerAnnual = headerRow.CreateCell(3);
            headerAnnual.SetCellValue("Yearly");
            headerAnnual.CellStyle = workbook.CreateCellStyle();
            headerAnnual.CellStyle.SetFont(font);

            AddSubscriptionRow(excelSheet1, 1, "AppStore", data.AppleStats);
            AddSubscriptionRow(excelSheet1, 2, "Google", data.GoogleStats);

            AddCounterRow(excelSheet2, 0, "at least one meal was added over the last week", data.ActiveUsers);

            workbook.Write(memoryStream);

            return memoryStream.ToArray();
        }

    }
}
