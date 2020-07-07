using MyFoodDoc.App.Application.Models;
using MyFoodDoc.App.Application.Payloads.User;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.App.Application.Abstractions
{
    public interface ICommonService
    {
        Task RegisterAsync(string email, string password, int insuranceId);

        Task<string> GeneratePasswordResetTokenAsync(string email);

        Task ResetPasswordAsync(string email, string resetToken, string newPassword);
    }
}
