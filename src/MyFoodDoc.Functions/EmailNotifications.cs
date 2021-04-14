using MyFoodDoc.Application.Abstractions;
using Microsoft.Azure.WebJobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using System.Net;

namespace MyFoodDoc.Functions
{
    public class EmailNotifications
    {
        private readonly IApplicationContext _context;
        private readonly IPdfService _pdfService;
        private readonly IEmailService _emailService;
        private readonly string _templateUrl;

        public EmailNotifications(IConfiguration configuration, IApplicationContext context, IPdfService pdfService, IEmailService emailService)
        {
            _context = context;
            _pdfService = pdfService;
            _emailService = emailService;

            var connectionString = configuration.GetConnectionString("BlobStorageConnection");

            var cloudStorageUrl = CloudStorageAccount.Parse(connectionString).BlobStorageUri.PrimaryUri.AbsoluteUri;

            _templateUrl = cloudStorageUrl + @"templates/teilnahmebescheinigung-praeventionskurs.pdf";
        }

        [FunctionName("CompletedCoursesEmailNotifications")]
        public async Task RunCompletedCoursesEmailNotificationsAsync(
            [TimerTrigger("0 */1 * * * *" /*"%TimerInterval%"*/, RunOnStartup = true)]
            TimerInfo myTimer,
            ILogger log,
            CancellationToken cancellationToken)
        {
            log.LogInformation("CompletedCoursesEmailNotifications. Start");

            var usersToNotify = await _context.CompletedCourses
                .Include(x => x.User)
                .Where(x => !x.NotificationSent)
                .ToListAsync(cancellationToken);

            log.LogInformation($"Users to notify: {usersToNotify.Count()}");

            int errors = 0;

            if (usersToNotify.Any())
            {
                byte[] bytes = null;

                using (WebClient client = new WebClient())
                {
                    var template = client.DownloadData(_templateUrl);

                    bytes = _pdfService.ReplaceText(template, "xx.mm.yyyy", DateTime.Now.ToString("dd.MM.yyyy"));
                }

                foreach (var user in usersToNotify)
                {
                    var result = await _emailService.SendEmailAsync(
                        user.User.Email,
                        "Test subject",
                        "Test body",
                        new[] {
                            new Attachment()
                            {
                                Content = bytes,
                                Filename = "teilnahmebescheinigung-praeventionskurs.pdf",
                                Type = "application/pdf"
                            }
                        });

                    if (!result)
                    {
                        errors++;

                        log.LogError($"Unable to send a email to {user.User.Email}");

                        continue;
                    }

                    user.NotificationSent = true;
                }

                _context.CompletedCourses.UpdateRange(usersToNotify);

                await _context.SaveChangesAsync(cancellationToken);
            }

            if (errors > 0)
                log.LogError($"{errors} users wasn't notified.");

            log.LogInformation("CompletedCoursesEmailNotifications. End");
        }
    }
}
