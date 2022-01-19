using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyFoodDoc.Application.Abstractions
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string email, string subject, string body, Attachment[] attachments = null);

        Task SendEmailToMultipleUsersAsync(string emailList, string subject, string body, Abstractions.Attachment[] attachments = null);
    }

    public class Attachment
    {
        public byte[] Content { get; set; }
        public string Type { get; set; }
        public string Filename { get; set; }
    }
}
