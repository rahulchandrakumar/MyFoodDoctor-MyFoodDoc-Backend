using System.Threading.Tasks;

namespace MyFoodDoc.Application.Abstractions
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string email, string[] bccEmails, string subject, string body, Attachment[] attachments = null);
    }

}
