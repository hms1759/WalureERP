using DataAccess.Model.OpenIddict;
using Iposweb.Core.Helpers;
using OpenIddict.Abstractions;
using OpenIddict.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onboarding.Component.OAuth
{
    /// <summary>
    /// OpendIddict Manager
    /// </summary>
    public class OpenIddictManager
    {
        public static async Task CreateClientApps(string basePath, OpenIddictApplicationManager<WalureOpenIddictApplication> manager,
            CancellationToken cancellationToken)
        {   var webApp = new WalureOpenIddictApplication
                {
                    AppId = OpenIddictClientAppHelper.WALURE_WEB_ID,
                    Language = "en-GB"
                };

                var webAppDescriptor = new OpenIddictApplicationDescriptor
                {
                    ClientId = OpenIddictClientAppHelper.WALURE_WEB_ID,
                    DisplayName = OpenIddictClientAppHelper.WALURE_WEB_NAME,
                    PostLogoutRedirectUris = { new Uri($"{basePath}/api/auth/token") },
                    RedirectUris = { new Uri($"{basePath}/api/auth/token") },
                    Type = OpenIddictConstants.ClientTypes.Confidential,
                    Permissions =  {
                        OpenIddictConstants.Permissions.Endpoints.Token,
                        OpenIddictConstants.Permissions.GrantTypes.Password,
                        OpenIddictConstants.Permissions.GrantTypes.RefreshToken,
                        OpenIddictConstants.Permissions.Scopes.Email,
                        OpenIddictConstants.Permissions.Scopes.Profile,
                        OpenIddictConstants.Permissions.Scopes.Roles,
                        OpenIddictConstants.Permissions.Endpoints.Revocation
                    }
                };

                await manager.PopulateAsync(webApp, webAppDescriptor, cancellationToken);
                await manager.CreateAsync(webApp, OpenIddictClientAppHelper.WALURE_WEB_SECRET, cancellationToken);
            
        }
    }
}
