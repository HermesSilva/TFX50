using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

using Scalar.AspNetCore;

using TFX.Core;
using TFX.Core.Controllers;
using TFX.Core.Extensions;
using TFX.Core.Identity;
using TFX.Core.IDs;
using TFX.Core.Interfaces;

public class XCustomControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
{
    public void PopulateFeature(IEnumerable<ApplicationPart> pParts, ControllerFeature pFeature)
    {
        var assemblys = AppDomain.CurrentDomain.GetAssemblies().Where(a => a.FullName.StartsWith("Sittax") || a.FullName.StartsWith("TFX")).ToList();
        foreach (var assembly in assemblys)
        {

            var controllers = assembly.GetTypes().Where(t => t.IsClass && !t.IsAbstract && typeof(XController).IsAssignableFrom(t));

            foreach (var controller in controllers)
            {
                if (!pFeature.Controllers.Contains(controller))
                {
                    pFeature.Controllers.Add(controller.GetTypeInfo());
                }
            }
        }
    }
}
public class XGuidUpperCaseConverter : JsonConverter<Guid>
{
    public override Guid Read(ref Utf8JsonReader pReader, Type pTypeToConvert, JsonSerializerOptions pOptions)
    {
        return Guid.Parse(pReader.GetString()!);
    }

    public override void Write(Utf8JsonWriter pWriter, Guid pValue, JsonSerializerOptions pOptions)
    {
        pWriter.WriteStringValue(pValue.ToString().ToUpperInvariant());
    }
}

public static class XServiceExtensions
{

    public static IApplicationBuilder AddDependencies(this IApplicationBuilder pApp)
    {
        pApp.UseMiddleware<XCustomMiddleware>();
        return pApp;
    }

    public static WebApplicationBuilder AddDependencies(this WebApplicationBuilder pBuilder)
    {
        var assemblys = AppDomain.CurrentDomain.GetAssemblies().ToList();
        var mvcBuilder = pBuilder.Services.AddControllers().AddJsonOptions(pOptions =>
        {
            pOptions.JsonSerializerOptions.PropertyNamingPolicy = null; // Mantém PascalCase
            pOptions.JsonSerializerOptions.Converters.Add(new XGuidUpperCaseConverter());
        });

        mvcBuilder.ConfigureApplicationPartManager(pApm =>
        {
            pApm.ApplicationParts.Add(new AssemblyPart(typeof(XCustomControllerFeatureProvider).Assembly));
            pApm.FeatureProviders.Add(new XCustomControllerFeatureProvider());
        });

        foreach (var assembly in assemblys)
        {
            var types = assembly.GetTypes();
            foreach (var type in types.Where(t => !t.IsAbstract && t.Implemnts<XIScoped>()))
            {
                var iface = type.GetInterfaces().FirstOrDefault(i => type.BaseType != null && type.BaseType.GetInterfaces().All(si => si != i));
                Console.WriteLine(iface?.FullName + " " + type.FullName);
                if (iface != null)
                    pBuilder.Services.AddScoped(iface, type);
                else
                    pBuilder.Services.AddTransient(type);
            }
        }

        var intef = typeof(XIModule);

        var implementations = assemblys.SelectMany(a => a.GetTypes()).Where(t => intef.IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);
        foreach (var tp in implementations)
        {
            var mdl = tp.CreateInstance<XIModule>();
            mdl.Initialize(pBuilder.Services);
        }
        return pBuilder;
    }

    public static void ConfigureServices(this IServiceCollection pServices)
    {
        pServices.AddJWT();
        pServices.AddCors(pOptions =>
        {
            pOptions.AddDefaultPolicy(b => b.AllowAnyOrigin()
                                           .AllowAnyMethod()
                                           .AllowAnyHeader()
                                           .WithExposedHeaders("*"));
        });

        pServices.AddControllers();
        pServices.AddEndpointsApiExplorer();
        pServices.AddControllers(pOptions =>
        {
            pOptions.Filters.Add<XResponseWrapperFilter>();
        }).ConfigureApiBehaviorOptions(pOptions =>
        {
            pOptions.InvalidModelStateResponseFactory = pContext =>
            {
                var err = string.Join("\r\n", pContext.ModelState.Values.SelectMany(a => a.Errors).Select(e => e.ErrorMessage));
                return new BadRequestObjectResult(XResponse.BadJSon + "\r\n" + err);
            };
        }).AddJsonOptions(pOptions =>
        {
            pOptions.JsonSerializerOptions.NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals;
        });

        pServices.AddRouting();
        pServices.AddAuthentication(XDefault.JWTKey)

        .AddCookie(XDefault.JWTKey, o =>
        {
            o.LoginPath = "/Access/Login";
            o.Cookie.Name = XDefault.JWTKey;
            o.Cookie.Path = "/";
        });
        pServices.Configure<KestrelServerOptions>(pOptions =>
        {
            pOptions.AllowSynchronousIO = true;
        });
        pServices.AddSingleton<XILoginService, XLoginService>();
        pServices.AddSingleton<XResponseWrapperFilter>();
        pServices.AddScoped<XITenantProvider, XTenantProvider>();
        pServices.AddScoped<XISharedTransaction, XSharedTransaction>();
        pServices.AddSingleton<XCustomMiddleware>();
    }

    public static void AddJWT(this IServiceCollection pService)
    {
        pService.AddHttpContextAccessor();
        pService.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(opt =>
        {
            opt.RequireHttpsMetadata = false;
            opt.SaveToken = true;
            opt.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,

                ValidateAudience = true,

                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                RequireExpirationTime = true,
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKeyResolver = (token, securityToken, kid, validationParameters) =>
                {
                    SecurityKey issuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(XDefault.JWTKey));
                    return new List<SecurityKey>() { issuerSigningKey };
                },
                NameClaimType = "Tootega.TFX.Core.ID.Claim",
                AudienceValidator = AudienceValidator,
                IssuerValidator = IssuerValidator
            };
            opt.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                    context.Token = token;
                    return Task.CompletedTask;
                }
            };
        });
    }

    private static string IssuerValidator(string issuer, SecurityToken securitytoken, TokenValidationParameters validationparameters)
    {
        validationparameters.ValidIssuer = XDefault.Emissor;

        return string.Empty;
    }

    private static bool AudienceValidator(IEnumerable<string> audiences, SecurityToken securityToken, TokenValidationParameters validationParameters)
    {
        validationParameters.ValidAudiences = XDefault.ValidoEm;

        return true;
    }
    public static IServiceCollection UseOpenApi(this IServiceCollection pService)
    {
        pService.AddEndpointsApiExplorer();
        pService.AddOpenApi(opt =>
        {
            opt.OpenApiVersion = OpenApiSpecVersion.OpenApi3_0;
            opt.AddDocumentTransformer(new XApplyOpenApiVisibility());
        });

        // Configura o schema para usar PascalCase (mesmo padrão do serializador)
        pService.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.PropertyNamingPolicy = null; // PascalCase
        });

        return pService;
    }

    public static WebApplication AddScalar(this WebApplication pApp)
    {
        if (!(pApp.Environment.IsDevelopment() || XEnvironment.AtivarScalar))
            return pApp;

        pApp.MapOpenApi();
        pApp.MapScalarApiReference(o => o.WithTitle("Tootega ERP").WithTheme(ScalarTheme.Mars));
        return pApp;
    }
}
