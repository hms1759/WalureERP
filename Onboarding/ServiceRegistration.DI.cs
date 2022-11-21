using DataAccess.IServices;
using DataAccess.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Shared.Dapper.Interfaces;
using Shared.Dapper.Repository;
using System.Data;
using System.Reflection;

namespace Onboarding
{
    public static partial class ServiceRegistration
    {
        public static void RegisterServicesDI(IServiceCollection services)
        {
             services.AddTransient<IAccountServices, AccountServices>();
             
        }
    }
}
