// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Bhasha.Common;
using IdentityServer4.Models;
using System.Collections.Generic;
using System.Security.Claims;

namespace Bhasha.Identity
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope(Roles.Student, new[] { ClaimTypes.Role }),
                new ApiScope(Roles.Author, new[] { ClaimTypes.Role })
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientId = "Bhasha.Student.Api.Client",
                    ClientSecrets =
                    {// TODO load secret from configuration
                        new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256())
                    },

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { Roles.Student }
                },

                new Client
                {
                    ClientId = "Bhasha.Author.Api.Client",
                    ClientSecrets =
                    { // TODO load secret from configuration
                        new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256())
                    },

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { Roles.Author }
                },
            };
    }
}
