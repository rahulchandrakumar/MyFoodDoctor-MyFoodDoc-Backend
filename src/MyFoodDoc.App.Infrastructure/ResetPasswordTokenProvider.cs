using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MyFoodDoc.Application.Entities;

namespace MyFoodDoc.App.Infrastructure
{
    public class ResetPasswordTokenProvider : TotpSecurityStampBasedTokenProvider<User>
    {
        public const string ProviderKey = "ResetPassword";

        public override Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<User> manager, User user)
        {
            return Task.FromResult(false);
        }
    }
}
