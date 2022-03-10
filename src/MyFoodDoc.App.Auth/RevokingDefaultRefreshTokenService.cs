using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace MyFoodDoc.App.Auth
{
    public class RevokingDefaultRefreshTokenService : DefaultRefreshTokenService
    {
        public RevokingDefaultRefreshTokenService(
           IRefreshTokenStore refreshTokenStore,
           IProfileService profile,
           ISystemClock clock,
           ILogger<DefaultRefreshTokenService> logger)
           :
           base(refreshTokenStore, profile, clock, logger)
        {
        }

        /// <summary>
        /// Workaround: accept old / used Refresh token 
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        /// <seealso cref="https://stackoverflow.com/questions/67417891/how-to-revoke-refresh-tokens-if-a-onetimeonly-token-is-used-multiple-times-in-id"/>
        /// <seealso cref="https://github.com/IdentityServer/IdentityServer4/blob/3ff3b46698f48f164ab1b54d124125d63439f9d0/src/IdentityServer4/src/Services/Default/DefaultRefreshTokenService.cs#L154"/>
        protected override async Task<bool> AcceptConsumedTokenAsync(RefreshToken refreshToken)
        {


            // Revoke all refresh tokens for this user
            // await RefreshTokenStore.RemoveRefreshTokensAsync(refreshToken.SubjectId, refreshToken.ClientId);

            // Base impl
            return await new ValueTask<bool>(true);
        }
    }
}
