using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace MyFoodDoc.App.Auth
{
    public class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }

        // scopes define the API resources in your system
        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>
            {
                new ApiScope("myfooddoc_api", "MyFoodDoc.Api")
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("myfooddoc_api", "MyFoodDoc.Api")
                {
                    ApiSecrets = {
                        new Secret("secret".Sha256())
                    },
                    Scopes = { "myfooddoc_api" }
                }
            };
        }

        // clients want to access resources (aka scopes)
        public static IEnumerable<Client> GetClients()
        {
            // client credentials client
            return new List<Client>
            {
                new Client
                {
                    ClientId = "myfooddoc_app",
                    ClientName = "MyFoodDoc.App",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AccessTokenType = AccessTokenType.Reference,
                    RequireClientSecret = false,
                    AllowedScopes = {"myfooddoc_api", IdentityServerConstants.StandardScopes.OfflineAccess},
                    AccessTokenLifetime = 3600 * 7,
                    AllowOfflineAccess = true,
                    RefreshTokenExpiration = TokenExpiration.Absolute,
                    RefreshTokenUsage = TokenUsage.ReUse
                }
            };
        }
    }
}
