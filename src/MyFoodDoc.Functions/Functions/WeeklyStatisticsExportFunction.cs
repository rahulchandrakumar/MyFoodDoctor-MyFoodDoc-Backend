using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Configuration;
using MyFoodDoc.Functions.Abstractions;
using MyFoodDoc.Functions.Helpers;
using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace MyFoodDoc.Functions.Functions
{
    public class WeeklyStatisticsExportFunction
    {
        private readonly IUserStatsService _userStatsService;
        private readonly StatisticsExportOptions _settings;
        private readonly IEmailService _emailService;
        private readonly ILogger log;

        public WeeklyStatisticsExportFunction(
            IUserStatsService userStatsService,
            IOptions<StatisticsExportOptions> options,
            IEmailService emailService,
            ILoggerFactory loggerFactory)
        {
            _userStatsService = userStatsService;
            _settings = options.Value;
            _emailService = emailService;
            log = loggerFactory.CreateLogger<WeeklyStatisticsExportFunction>();
        }

        [Function("WeeklyStatisticsExport")]
        public async Task RunAsync(
            [TimerTrigger("0 10 0 * * MON", RunOnStartup = true)]
            TimerInfo myTimer)
        {
            var currentDate = DateTime.Now;

            if (myTimer.IsPastDue)
            {
                log.LogInformation("Timer is running late!");
            }

            if (String.IsNullOrEmpty(_settings.EmailList))
            {
                throw new ArgumentNullException(nameof(_settings.EmailList));
            }

            log.LogInformation($"WeeklyStatisticsExport executed at: {currentDate}");

            var data = await _userStatsService.GetUserStatsAsync();
            var bytes = ExcelHelper.CreateExcelFile(data, true);

            using (Stream bodyTemplateStream = Assembly.GetExecutingAssembly().
                    GetManifestResourceStream($"{typeof(Program).Namespace}.Templates.SubscriptionWeeklyStatisticsEmailTemplate.html"))
            {
                if (bodyTemplateStream == null)
                {
                    throw new ArgumentNullException(nameof(bodyTemplateStream));
                }

                using StreamReader reader = new StreamReader(bodyTemplateStream);
                string body = String.Format(reader.ReadToEnd(), currentDate.ToString("dd/MM/yyyy", new CultureInfo("de")));

                string[] list = _settings.EmailList.Split(',');

                string mainEmail = list[0];
                string[] bccEmailList = null;
                if (list.Length > 1)
                {
                    bccEmailList = new string[list.Length - 1];
                    Array.Copy(list, 1, bccEmailList, 0, list.Length - 1);
                }

                await _emailService.SendEmailAsync(
                    mainEmail,
                    bccEmailList,
                    "Weekly export sheet",
                    body,
                    new[] {
                            new Attachment()
                            {
                                Content = bytes,
                                Filename = $"Statistics {currentDate.ToString("MMMM yyyy", new CultureInfo("de"))}. Stand {currentDate:dd-MM-yyyy HH-mm}.xlsx",
                                Type = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                            }
                    });
            }
        }
    }
}
