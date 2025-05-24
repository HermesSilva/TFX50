using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;

using AspNetCore.Scalar;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

using TFX.Core;
using TFX.Core.Controllers;
using TFX.Core.Extensions;
using TFX.Core.Identity;
using TFX.Core.IDs;
using TFX.Core.Interfaces;
public class CustomControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
{
    public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
    {
        var assemblys = AppDomain.CurrentDomain.GetAssemblies().Where(a => a.FullName.StartsWith("Sittax") || a.FullName.StartsWith("TFX")).ToList();
        foreach (var assembly in assemblys)
        {

            var controllers = assembly.GetTypes().Where(t => t.IsClass && !t.IsAbstract && typeof(XController).IsAssignableFrom(t));

            foreach (var controller in controllers)
            {
                if (!feature.Controllers.Contains(controller))
                {
                    feature.Controllers.Add(controller.GetTypeInfo());
                }
            }
        }
    }
}
public class GuidUpperCaseConverter : JsonConverter<Guid>
{
    public override Guid Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return Guid.Parse(reader.GetString()!);
    }

    public override void Write(Utf8JsonWriter writer, Guid value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString().ToUpperInvariant());
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
        var assemblys = AppDomain.CurrentDomain.GetAssemblies().Where(a => a.FullName.StartsWith("TFX")).ToList();
        var files = Directory.GetFiles(XDefault.AppPath, "TFX.*.dll").Where(f => !assemblys.Any(a => Path.GetFileName(a.Location) == Path.GetFileName(f))).ToList();
        foreach (var file in files)
        {
            try
            {
                var asm = Assembly.LoadFrom(file);
                assemblys.Add(asm);
            }
            catch (Exception ex)
            {
            }
        }

        var mvcBuilder = pBuilder.Services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new GuidUpperCaseConverter());
        });

        mvcBuilder.ConfigureApplicationPartManager(apm =>
        {
            apm.ApplicationParts.Add(new AssemblyPart(typeof(CustomControllerFeatureProvider).Assembly));
            apm.FeatureProviders.Add(new CustomControllerFeatureProvider());
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
        pServices.AddCors(options =>
        {
            options.AddDefaultPolicy(b => b.AllowAnyOrigin()
                                           .AllowAnyMethod()
                                           .AllowAnyHeader()
                                           .WithExposedHeaders("*"));
        });

        pServices.AddControllers();
        pServices.AddEndpointsApiExplorer();
        pServices.AddSwaggerGen();
        pServices.AddControllers(options =>
        {
            options.Filters.Add<XResponseWrapperFilter>();
        }).ConfigureApiBehaviorOptions(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                return new BadRequestObjectResult(XResponse.BadJSon);
            };
        }).AddJsonOptions(options => { options.JsonSerializerOptions.PropertyNamingPolicy = null; });


        pServices.AddRouting();
        pServices.AddAuthentication(XDefault.JWTKey)

        .AddCookie(XDefault.JWTKey, o =>
        {
            o.LoginPath = "/Access/Login";
            o.Cookie.Name = XDefault.JWTKey;
            o.Cookie.Path = "/";
        });
        pServices.Configure<KestrelServerOptions>(options =>
        {
            options.AllowSynchronousIO = true;
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
                IssuerSigningKeyResolver = (token, secutiryToken, kid, validationParameters) =>
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
        });
        pService.AddSwaggerGen(c =>
        {
            c.CustomSchemaIds(type => type.FullName);
            c.SwaggerGeneratorOptions.DescribeAllParametersInCamelCase = true;
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
            {
                c.IncludeXmlComments(xmlPath);
                c.SchemaFilter<EnumSchemaFilter>(xmlPath);
            }
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = AppDomain.CurrentDomain.FriendlyName,
                Version = "v1",
                Description = $"API {AppDomain.CurrentDomain.FriendlyName}",
                Contact = new OpenApiContact
                {
                    Name = "Tootega Pesquisa e Inovação",
                    Url = new Uri("https://github.com/Tootega/"),
                    Email = "comercial@tootega.com.br"
                }
            });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "'Bearer' [spaço] seu token | Exemplo: Bearer ",
                Name = "Authorization",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "Bearer",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    },
                    new string[] {}
                }
            });
        });

        return pService;
    }

    public static WebApplication AddScalar(this WebApplication pApp)
    {
        if (!(pApp.Environment.IsDevelopment() || XEnvironment.AtivarScalar))
            return pApp;

        pApp.UseSwagger();
        pApp.UseScalar(options =>
        {
            options.RoutePrefix = "docs";
            options.DocumentTitle = $"API ({AppDomain.CurrentDomain.FriendlyName})";
            options.UseTheme(Theme.Mars);
        });
        pApp.MapOpenApi();
        return pApp;
    }
}
