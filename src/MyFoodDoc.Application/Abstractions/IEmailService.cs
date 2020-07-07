using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyFoodDoc.Application.Abstractions
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string email, string subject, string body);
    }
}
