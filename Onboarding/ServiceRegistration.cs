using DataAccess.Config;
using DataAccess.Model.Identity;
using DataAccess.Model.OpenIddict;
using DataAccess.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Onboarding.Component.OAuth;
using OpenIddict.Core;
using Serilog;
using Serilog.Events;
using Shared.Dapper.Interfaces;
using Shared.Dapper.Repository;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Onboarding
{
    public static partial class ServiceRegistration
    {
        public static WebApplicationBuilder RegisterServices(this WebApplicationBuilder builder)
        {
            var services = builder.Services;
            services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(option =>
            {
                // Set the comments path for the Swagger JSON and UI.    
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                option.IncludeXmlComments(xmlPath);

                option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
             
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description =
                    "JWT Authorization header using the Bearer scheme. \r\n\r\n " +
                    "Enter 'Bearer'  and then your token in the text input " +
                    "below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                option.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });

            });

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
                // to replace the default OpenIddict entities.
                options.UseOpenIddict<WalureOpenIddictApplication, WalureOpenIddictAuthorization, WalureOpenIddictScope, WalureOpenIddictToken, Guid>();


            });
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            services.AddSingleton<IDbConnection>(db =>
            {
                var connectionString = builder.Configuration.GetConnectionString("Default");
                var connection = new SqlConnection(connectionString);
                return connection;
            });
            services.AddIdentity<WalureUser, WalureRole>(option =>
            {
                option.Password.RequiredLength = 5;
                option.Password.RequireUppercase = true;
                option.Password.RequireDigit = true;
                option.Password.RequireLowercase = true;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            async Task InitializeAsync(IServiceProvider services, CancellationToken cancellationToken)
            {
                using (var scope = services.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    var manager = scope.ServiceProvider.GetRequiredService<OpenIddictApplicationManager<WalureOpenIddictApplication>>();
                    await OpenIddictManager.CreateClientApps(builder.Configuration["AuthSettings:Authority"], manager, cancellationToken);
                }
            }


            var authSettings = new AuthSettings();
            builder.Configuration.Bind(nameof(AuthSettings), authSettings);

            services.Configure<IdentityOptions>(options =>
            {
                options.ClaimsIdentity.UserNameClaimType = Claims.Name;
                options.ClaimsIdentity.UserIdClaimType = Claims.Subject;
                options.ClaimsIdentity.RoleClaimType = Claims.Role;
            });

            var tokenExpiry = TimeSpan.FromMinutes(authSettings.TokenExpiry);
            services.AddOpenIddict()
               .AddCore(options =>
               {
                   options.UseEntityFrameworkCore()
                       .UseDbContext<ApplicationDbContext>()
                        .ReplaceDefaultEntities<WalureOpenIddictApplication, WalureOpenIddictAuthorization, WalureOpenIddictScope, WalureOpenIddictToken, Guid>(); ;
               })
               .AddServer(options =>
               {
                   //options.AddEventHandler<ApplyTokenResponseContext>(builder =>
                   //builder.UseSingletonHandler<MyApplyTokenResponseHandler>());

                   options.RegisterScopes(Scopes.Email, Scopes.Profile, Scopes.Address, Scopes.Phone,
                       Scopes.Roles, Scopes.OfflineAccess, Scopes.OpenId);

                   if (!authSettings.RequireHttps)
                   {
                       options.UseAspNetCore(configure =>
                       {
                           configure.DisableTransportSecurityRequirement();
                       });

                       // Register the signing and encryption credentials.
                       options.AddDevelopmentEncryptionCertificate()
                              .AddDevelopmentSigningCertificate();
                   }
                   else
                   {
                       byte[] rawData = File.ReadAllBytes(Path.Combine(builder.Environment.ContentRootPath, "App_Data", "walure-uat-cert.pfx"));
                       var x509Certificate = new X509Certificate2(rawData,
                           authSettings.PrivateKey,
                           X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable);

                       options.AddEncryptionCertificate(x509Certificate).AddSigningCertificate(x509Certificate);
                   }

                   options.SetTokenEndpointUris("/api/auth/token")
                   // .SetUserinfoEndpointUris("/api/auth/userinfo")
                   //.SetRevocationEndpointUris("/api/auth/revoke")
                   .AllowRefreshTokenFlow()
                   .AcceptAnonymousClients()
                   .AllowPasswordFlow()
                   .SetAccessTokenLifetime(tokenExpiry)
                   .SetIdentityTokenLifetime(tokenExpiry)
                   .SetRefreshTokenLifetime(tokenExpiry);

                   options.UseAspNetCore()
                        .EnableAuthorizationEndpointPassthrough()
                        .EnableLogoutEndpointPassthrough()
                        .EnableStatusCodePagesIntegration()
                        .EnableTokenEndpointPassthrough();
               });

               //// Register the OpenIddict validation components.
               //.AddValidation(options =>
               //{
               //    // Import the configuration from the local OpenIddict server instance.
               //    options.UseLocalServer();

               //    // Register the ASP.NET Core host.
               //    options.UseAspNetCore();
               //});

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;

                cfg.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = authSettings.Issuer,
                    ValidAudience = authSettings.Issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authSettings.SecretKey)),
                };
            });


            RegisterServicesDI(services);
           // RegisterServicesAuth(services, builder.Configuration);

            //Serilog COnfiguration
            builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
            {
                var logConfig = loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration)
                                    .Enrich.FromLogContext()
                                    .WriteTo.File(@"logs\log.txt", rollingInterval: RollingInterval.Day,
                                    restrictedToMinimumLevel: LogEventLevel.Information,
                                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                                    shared: true);


            });

            return builder;
        }

    }
}
