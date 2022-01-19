using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Configuration;
using MyFoodDoc.Core;
using MyFoodDoc.Functions.Abstractions;
using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace MyFoodDoc.Functions
{    
    public class WeeklyStatisticsExportFunction
    {
        private readonly IUserStatsService _userStatsService;
        private readonly StatisticsExportOptions _settings;
        private readonly IEmailService _emailService;

        public WeeklyStatisticsExportFunction(
            IUserStatsService userStatsService,
            IOptions<StatisticsExportOptions> options,
            IEmailService emailService)
        {
            _userStatsService = userStatsService;
            _settings = options.Value;
            _emailService = emailService;
        }

        [FunctionName("WeeklyStatisticsExport")]
        public async Task RunAsync(
            [TimerTrigger("0 10 0 * * MON", RunOnStartup = false)]
            TimerInfo myTimer,
            ILogger log)
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

            Stream bodyTemplateStream = Assembly.GetExecutingAssembly().
                GetManifestResourceStream($"{this.GetType().Namespace}.Templates.SubscriptionWeeklyStatisticsEmailTemplate.html");

            if (bodyTemplateStream == null)
            {
                throw new ArgumentNullException(nameof(bodyTemplateStream));
            }

            StreamReader reader = new StreamReader(bodyTemplateStream);
            string body = String.Format(reader.ReadToEnd(), currentDate.ToString("dd/MM/yyyy", new CultureInfo("de")));

            await _emailService.SendEmailToMultipleUsersAsync(
                _settings.EmailList,
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
