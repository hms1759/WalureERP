//using Microsoft.AspNetCore.Authentication.Cookies;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Hosting.Internal;
//using Microsoft.IdentityModel.Tokens;
//using Microsoft.OpenApi.Models;
//using Onboarding.Component.OAuth;
//using OpenIddict.Core;
//using System.Reflection;
//using static OpenIddict.Abstractions.OpenIddictConstants;
//using System.Security.Cryptography.X509Certificates;
//using System.Text;
//using DataAccess.ViewModels;
//using DataAccess.Config;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using DataAccess.Model.OpenIddict;
//using System.Net;

//namespace Onboarding
//{
//    public static partial class ServiceRegistration
//    {
//        public static void RegisterServicesAuth(IServiceCollection services, ConfigurationManager configuration)
//        {

//            async Task InitializeAsync(IServiceProvider services, CancellationToken cancellationToken)
//            {
//                using (var scope = services.GetRequiredService<IServiceScopeFactory>().CreateScope())
//                {
//                    var manager = scope.ServiceProvider.GetRequiredService<OpenIddictApplicationManager<WalureOpenIddictApplication>>();
//                    await OpenIddictManager.CreateClientApps(configuration["AuthSettings:Authority"], manager, cancellationToken);
//                }
//            }


//            var authSettings = new AuthSettings();
//           configuration.Bind(nameof(AuthSettings), authSettings);

//            services.Configure<IdentityOptions>(options =>
//            {
//                options.ClaimsIdentity.UserNameClaimType = Claims.Name;
//                options.ClaimsIdentity.UserIdClaimType = Claims.Subject;
//                options.ClaimsIdentity.RoleClaimType = Claims.Role;
//            });

//            var tokenExpiry = TimeSpan.FromMinutes(authSettings.TokenExpiry);
//            services.AddOpenIddict()
//               .AddCore(options =>
//               {
//                   options.UseEntityFrameworkCore()
//                       .UseDbContext<ApplicationDbContext>()
//                        .ReplaceDefaultEntities<WalureOpenIddictApplication, WalureOpenIddictAuthorization, WalureOpenIddictScope, WalureOpenIddictToken, Guid>(); ;
//               })
//               .AddServer(options =>
//               {
//                   //options.AddEventHandler<ApplyTokenResponseContext>(builder =>
//                   //builder.UseSingletonHandler<MyApplyTokenResponseHandler>());

//                   options.RegisterScopes(Scopes.Email, Scopes.Profile, Scopes.Address, Scopes.Phone,
//                       Scopes.Roles, Scopes.OfflineAccess, Scopes.OpenId);

//                   if (!authSettings.RequireHttps)
//                   {
//                       options.UseAspNetCore(configure =>
//                       {
//                           configure.DisableTransportSecurityRequirement();
//                       });

//                       // Register the signing and encryption credentials.
//                       options.AddDevelopmentEncryptionCertificate()
//                              .AddDevelopmentSigningCertificate();
//                   }
//                   else
//                   {
//                       byte[] rawData = File.ReadAllBytes(Path.Combine(Environment.ContentRootPath, "App_Data", "walure-uat-cert.pfx"));
//                       var x509Certificate = new X509Certificate2(rawData,
//                           authSettings.PrivateKey,
//                           X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable);

//                       options.AddEncryptionCertificate(x509Certificate).AddSigningCertificate(x509Certificate);
//                   }

//                   options.SetTokenEndpointUris("/api/auth/token")
//                   // .SetUserinfoEndpointUris("/api/auth/userinfo")
//                   //.SetRevocationEndpointUris("/api/auth/revoke")
//                   .AllowRefreshTokenFlow()
//                   .AcceptAnonymousClients()
//                   .AllowPasswordFlow()
//                   .SetAccessTokenLifetime(tokenExpiry)
//                   .SetIdentityTokenLifetime(tokenExpiry)
//                   .SetRefreshTokenLifetime(tokenExpiry);

//                   options.UseAspNetCore()
//                        .EnableAuthorizationEndpointPassthrough()
//                        .EnableLogoutEndpointPassthrough()
//                        .EnableStatusCodePagesIntegration()
//                        .EnableTokenEndpointPassthrough();
//               });

//            //// Register the OpenIddict validation components.
//            //.AddValidation(options =>
//            //{
//            //    // Import the configuration from the local OpenIddict server instance.
//            //    options.UseLocalServer();

//            //    // Register the ASP.NET Core host.
//            //    options.UseAspNetCore();
//            //});

//            services.AddAuthentication(x =>
//            {
//                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
//            })
//            .AddJwtBearer(cfg =>
//            {
//                cfg.RequireHttpsMetadata = false;
//                cfg.SaveToken = true;

//                cfg.TokenValidationParameters = new TokenValidationParameters()
//                {
//                    ValidateIssuer = true,
//                    ValidateAudience = true,
//                    ValidateLifetime = true,
//                    ValidateIssuerSigningKey = true,
//                    ValidIssuer = authSettings.Issuer,
//                    ValidAudience = authSettings.Issuer,
//                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authSettings.SecretKey)),
//                };
//            });


//        }

//    }
//}
