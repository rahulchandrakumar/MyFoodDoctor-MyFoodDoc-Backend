using Microsoft.AspNetCore.Identity;
using MyFoodDoc.Application.Entities;
using System.Threading.Tasks;

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
