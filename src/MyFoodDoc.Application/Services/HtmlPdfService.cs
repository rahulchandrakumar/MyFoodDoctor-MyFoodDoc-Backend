using MyFoodDoc.Application.Abstractions;
using System.Threading.Tasks;
using System.IO;
using PuppeteerSharp;
using PuppeteerSharp.Media;
using MyFoodDoc.Application.Entities.Html;

namespace MyFoodDoc.Application.Services
{
    public interface IHtmlPdfService
    {
        Task<byte[]> GenerateExport(DiaryExportModel data, HtmlStructure htmlStruct);

    }

    public class HtmlPdfService : IHtmlPdfService
    {
        public async Task<byte[]> GenerateExport(DiaryExportModel data, HtmlStructure htmlStruct)
        {
            var html = HtmlParser.ParseHtml(data, htmlStruct);
            // await new BrowserFetcher().DownloadAsync();

            using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true,
                Args = new[]
                {
                    "--no-sandbox"
                }
            });

            using var page = await browser.NewPageAsync();
            await page.SetContentAsync(html);
            var result = await page.GetContentAsync();
            return await page.PdfDataAsync(new PdfOptions()
            {
                Landscape = true,
                DisplayHeaderFooter = true,
                HeaderTemplate = htmlStruct.PdfHeader,
                FooterTemplate = htmlStruct.PdfFooter,
                Format = PaperFormat.A4,
                PrintBackground = true,
                MarginOptions = new MarginOptions()
                {
                    Top = htmlStruct.MarginOptions.Top,
                    Bottom = htmlStruct.MarginOptions.Bottom,
                    Left = htmlStruct.MarginOptions.Left,
                    Right = htmlStruct.MarginOptions.Right,
                }
            });
        }
    }
}
