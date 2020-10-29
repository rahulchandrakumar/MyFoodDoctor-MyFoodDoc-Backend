using System;
using System.Collections.Generic;
using System.IO;
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

        public async Task<bool> SendEmailAsync(string email, string subject, string body)
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
