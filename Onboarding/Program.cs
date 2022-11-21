
using Onboarding;

var app = WebApplication.CreateBuilder(args)
                        .RegisterServices()
                        .Build();
app.SetupMiddleware((ConfigurationManager)app.Configuration)
    .Run();

