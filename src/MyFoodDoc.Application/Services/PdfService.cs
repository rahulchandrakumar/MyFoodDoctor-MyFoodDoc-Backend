using MyFoodDoc.Application.Abstractions;
using Spire.Pdf;
using Spire.Pdf.General.Find;
using System.Drawing;
using Spire.Pdf.Graphics;
using System.IO;
using System;

namespace MyFoodDoc.Application.Services
{
    public class PdfService : IPdfService
    {
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
                PdfFont font = new PdfFont(PdfFontFamily.Helvetica, 10f, PdfFontStyle.Regular);

                RectangleF rec;
                foreach (PdfTextFind find in collection.Finds)
                {
                    rec = find.Bounds;
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
