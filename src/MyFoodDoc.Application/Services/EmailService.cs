using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyFoodDoc.Application.Abstractions;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace MyFoodDoc.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailServiceOptions _options;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<EmailServiceOptions> options,
            ILogger<EmailService> logger)
        {
            _options = options.Value;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task SendEmailToMultipleUsersAsync(string emailList, string subject, string body, Abstractions.Attachment[] attachments = null)
        {
            var tasks = emailList.Split(',').Select(x => SendEmailAsync(x, subject, body, attachments));

            await Task.WhenAll(tasks);
        }

        public async Task<bool> SendEmailAsync(string email, string subject, string body, Abstractions.Attachment[] attachments = null)
        {
            var sendGridClient = new SendGridClient(_options.SendGridApiKey);

            // Configure email
            var sendGridMail = new SendGridMessage
            {
                From = new EmailAddress(_options.FromAddress, _options.FromName),
                Personalizations = new List<Personalization> {
                        new Personalization {
                            Tos = new List<EmailAddress> {new EmailAddress(email)},
                            Subject = subject
                        }
                    },
                PlainTextContent = body,
                HtmlContent = body
            };

            if (attachments != null)
            {
                sendGridMail.Attachments = new List<SendGrid.Helpers.Mail.Attachment>();

                foreach (var attachment in attachments)
                {
                    sendGridMail.Attachments.Add(
                    new SendGrid.Helpers.Mail.Attachment
                    {
                        Content = Convert.ToBase64String(attachment.Content),
                        Filename = attachment.Filename,
                        Type = attachment.Type,
                        Disposition = "attachment"
                    });
                }
            }

            // Send email
            Response response = await sendGridClient.SendEmailAsync(sendGridMail);

            // Process response
            using (var streamReader = new StreamReader(await response.Body.ReadAsStreamAsync()))
            {
                string mailResult = await streamReader.ReadToEndAsync();

                if (response.StatusCode == HttpStatusCode.Accepted || response.StatusCode == HttpStatusCode.OK)
                    return true;
                else
                {
                    _logger.LogError(mailResult);
                    return false;
                }
            }
        }
    }
}
