using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using MyFoodDoc.App.Application.Abstractions;
using MyFoodDoc.App.Application.Models;
using MyFoodDoc.App.Application.Payloads.Diary;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.Functions.Windows.Functions
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

        [Function(nameof(ExportPdf))]
        public async Task RunAsync([QueueTrigger(App.Application.Constants.Export.PdfQueue)] DiaryQueueItem queuePayload)
        {
            _logger.LogInformation($"Start export pdf for: {JsonSerializer.Serialize(queuePayload)}");

            await _service.ExportAsync(queuePayload.UserId, new ExportPayload
            {
                DateFrom = queuePayload.DateFrom,
                DateTo = queuePayload.DateTo
            }, CancellationToken.None);

            _logger.LogInformation($"End export pdf");
        }
    }
}
