using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using MyFoodDoc.App.Application.Abstractions;
using MyFoodDoc.App.Application.Models;
using MyFoodDoc.App.Application.Payloads.Diary;
using MyFoodDoc.Application.Entities.Html;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MyFoodDoc.Functions.Functions
{
    public class ExportPdf
    {
        private readonly ILogger _logger;
        private readonly IDiaryService _service;

        public ExportPdf(ILoggerFactory loggerFactory,
            IDiaryService service)
        {
            _logger = loggerFactory.CreateLogger<ExportPdf>();
            _service = service;
        }

        [FunctionName(nameof(ExportPdf))]
        public async Task RunAsync([QueueTrigger(App.Application.Constants.Export.PdfQueue)] DiaryQueueItem queuePayload,
            [Blob("html-pdf-template/html-structure.json", FileAccess.Read)] string htmlStructure )
        {
            _logger.LogInformation($"Start export pdf for: {JsonConvert.SerializeObject(queuePayload)}");
          
            var htmlStruct = JsonConvert.DeserializeObject<HtmlStructure>(htmlStructure);
            await _service.ExportAsync(queuePayload.UserId, new ExportPayload
            {
                DateFrom = queuePayload.DateFrom,
                DateTo = queuePayload.DateTo,
                HtmlStruct = htmlStruct,
            }, CancellationToken.None);

            _logger.LogInformation($"End export pdf");
        }
    }
}
