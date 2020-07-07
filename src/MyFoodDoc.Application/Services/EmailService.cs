using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using MyFoodDoc.Application.Abstractions;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace MyFoodDoc.Application.Services
{
    public class EmailService : IEmailService
    {
        public const string FromAddress = "app@myfooddoctor.de";
        public const string FromName = "My food doctor";
        public const string SendGridApiKey = "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";
        
        public async Task<bool> SendEmailAsync(string email, string subject, string body)
        {
            var sendGridClient = new SendGridClient(SendGridApiKey);

            // Configure email
            var sendGridMail = new SendGridMessage
            {
                From = new EmailAddress(FromAddress, FromName),
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
                    return false;
            }
        }
    }
}
