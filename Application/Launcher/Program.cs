using System;
using System.Configuration;
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
using TFX.Core.Data.DB;
using TFX.Core.Identity;
using TFX.Core.IDs;
using TFX.Core.Interfaces;
using TFX.ESC.Core;
using TFX.ESC.Core.DB;
namespace Launcher
{
    public class Program
    {
        public static WebApplication App;
        public static bool IsAsync;

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(new WebApplicationOptions
            {
                Args = args,
                ContentRootPath = "/Tootega/Source/TFX50/Core/TFX.Core.UI"
            });
            builder.Services.UseOpenApi();
            builder.Services.ConfigureServices();

            builder.Services.AddDbContext<TFXCoreDataContext>();
            builder.Services.AddDbContext<TFXESCCoreContext>();
            Console.WriteLine(typeof(TFXESCCoreModule).FullName);
            builder.AddDependencies();
            App = builder.Build();

            App.AddDependencies();
            XEnvironment.Services = App.Services;
            
            App.UseAuthorization();
            App.UseAuthentication();
            App.MapControllers();
            App.UseStaticFiles();
            App.AddScalar();
            XSessionManager.Initialize(App.Services);
            if (IsAsync)
                App.RunAsync("http://+:7000");
            else
                App.Run("http://+:7000");
        }
    }
}
