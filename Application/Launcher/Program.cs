using System;
using System.Text.Json;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using TFX.Core;
using TFX.Core.Cache;
using TFX.Core.Controllers;
using TFX.Core.Identity;
using TFX.Core.IDs;
using TFX.Core.Interfaces;
namespace Launcher
{
    public class Program
    {
        public static WebApplication App;

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(new WebApplicationOptions
            {
                Args = args,
                WebRootPath = "/Tootega/Source/Access-POC/App/Launchers/WebUI/dist/ef6-angular-poc"
            });
            builder.Services.UseOpenApi();


            builder.Services.ConfigureServices();
            builder.Services.AddSingleton<XILoginService, XLoginService>();
            builder.Services.AddScoped<XITenantProvider, XTenantProvider>();
            builder.Services.AddScoped<XISharedTransaction, XSharedTransaction>();

            builder.AddDependencies();
            App = builder.Build();
            XEnvironment.Services = App.Services;

            App.UseCors();
            App.UseAuthorization();
            App.UseAuthentication();
            App.MapControllers();
            App.UseStaticFiles();
            App.AddScalar();
            XSessionManager.Initialize(App.Services);
            App.Run("http://+:7000");
        }


    }
}
