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
using TFX.Core.Data;
using TFX.Core.Data.CEP;
using TFX.Core.Data.CEP.DataPack;
using TFX.Core.Data.DB;
using TFX.Core.Identity;
using TFX.Core.IDs;
using TFX.Core.Interfaces;
using TFX.ESC.Core;
using TFX.ESC.Core.DB;

using Tootega.Core.CEP;
using Tootega.Core.ERP;
namespace Launcher
{
    public class Program
    {
        public static WebApplication App;
        public static bool IsAsync = false;

        public static void Main(string[] pArgs)
        {
            var builder = WebApplication.CreateBuilder(new WebApplicationOptions
            {
                Args = pArgs,
                ContentRootPath = "/Tootega/Source/TFX50/Core/TFX.Core.UI"
            });
            builder.Services.UseOpenApi();
            builder.Services.ConfigureServices();
            builder.Services.AddCors(pOptions =>
            {
                pOptions.AddDefaultPolicy(pOlicy =>
                {
                    pOlicy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });
            builder.Services.AddDbContext<TFXCoreDataContext>();
            builder.Services.AddDbContext<TFXESCCoreContext>();
            builder.Services.AddDbContext<CEPxDBContext>();
            Console.WriteLine(typeof(TFXESCCoreModule).FullName);
            Console.WriteLine(typeof(TootegaCoreCEPModule).FullName);
            Console.WriteLine(typeof(TFXESCCoreModule).FullName);
            Console.WriteLine(typeof(TFXCoreDataModule).FullName);
            Console.WriteLine(typeof(TootegaCoreERPModule).FullName);
            builder.AddDependencies();
            App = builder.Build();

            App.AddDependencies();
            XEnvironment.Services = App.Services;
            App.UseCors();
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
            //CEPxDataPack.Apply();
            //Console.ReadLine();
        }
    }
}
