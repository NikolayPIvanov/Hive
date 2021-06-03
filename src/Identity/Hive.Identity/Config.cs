// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.


using Duende.IdentityServer.Models;
using System.Collections.Generic;
using Duende.IdentityServer;

namespace Hive.Identity
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };

        public static IEnumerable<ApiResource> ApiResources =>
            new ApiResource[]
            {
                new ApiResource("gigs-management", "Gigs Management API")
                {
                    Scopes = { "gigs.read", "gigs.write", "gigs.delete" }
                },
                
                new ApiResource("ordering", "Orders Management API")
                {
                    Scopes = { "ordering.read", "ordering.write", "ordering.delete" }
                }
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("gigs.read"),
                new ApiScope("gigs.write"),
                new ApiScope("gigs.delete"),
                
                new ApiScope("ordering.read"),
                new ApiScope("ordering.write"),
                new ApiScope("ordering.delete")
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                // interactive client using code flow + pkce
                new Client
                {
                    ClientId = "interactive",
                    ClientSecrets = {new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256())},

                    AllowedGrantTypes = GrantTypes.Code,
                    AccessTokenLifetime = 365 * 24 * 60,

                    RedirectUris = { "https://localhost:44300/signin-oidc" },
                    FrontChannelLogoutUri = "https://localhost:44300/signout-oidc",
                    PostLogoutRedirectUris = {"https://localhost:44300/signout-callback-oidc"},

                    AllowOfflineAccess = true,
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        
                        "gigs.read",
                        "gigs.write",
                        "gigs.delete",
                        
                        "ordering.read",
                        "ordering.write",
                        "ordering.delete"
                        
                    }
                    
                },
                
                new Client
                {
                    ClientName = "Angular-Client",
                    ClientId = "angular-client",
                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris = new List<string>
                    {
                        "http://localhost:4200/auth/signin-callback"
                    },
                    PostLogoutRedirectUris = { "http://localhost:4200/auth/signout-callback" },
                    RequirePkce = true,
                    AllowAccessTokensViaBrowser = true,
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        
                        "gigs.read",
                        "gigs.write",
                        "gigs.delete",
                        
                        "ordering.read",
                        "ordering.write",
                        "ordering.delete"
                    },
                    
                    AllowedCorsOrigins = { "http://localhost:4200" },
                    RequireClientSecret = false,
                    RequireConsent = false,
                    AccessTokenLifetime = 600
                }
            };
    }
}