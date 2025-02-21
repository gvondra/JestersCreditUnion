using Autofac;
using Autofac.Extensions.DependencyInjection;
using BrassLoon.Extensions.Logging;
using JestersCreditUnion.CommonAPI;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
#if !DEBUG
using Microsoft.Extensions.Logging;
#endif
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using System;

namespace API
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            builder.Host.ConfigureContainer((ContainerBuilder builder) => builder.RegisterModule(new APIModule()));

            // Add services to the container.
            builder.Services.Configure<Settings>(builder.Configuration);

            builder.Services.AddLogging(b =>
            {
#if !DEBUG
                b.ClearProviders();
#endif
                Settings settings = new Settings();
                builder.Configuration.Bind(settings);
                if (settings.LogDomainId.HasValue && !string.IsNullOrEmpty(settings.BrassLoonLogRpcBaseAddress) && settings.BrassLoonLogClientId.HasValue)
                {
                    b.AddBrassLoonLogger(c =>
                    {
                        c.LogApiBaseAddress = settings.BrassLoonLogRpcBaseAddress;
                        c.LogDomainId = settings.LogDomainId.Value;
                        c.LogClientId = settings.BrassLoonLogClientId.Value;
                        c.LogClientSecret = settings.BrassLoonLogClientSecret;
                    });
                }
            });

            builder.Services.AddControllers(options =>
            {
                // options.InputFormatters.Add(new CsvTextInputFormatter());
                // options.OutputFormatters.Add(new CsvTextOutputFormatter());
            })
                .AddNewtonsoftJson(o =>
                {
                    o.SerializerSettings.ContractResolver = new DefaultContractResolver();
                })
                .AddJsonOptions(o =>
                {
                    o.JsonSerializerOptions.PropertyNamingPolicy = null;
                });
            builder.Services.AddCors(builder.Configuration);
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(o =>
            {
                o.SwaggerDoc(
                    "v1",
                    new OpenApiInfo
                    {
                        Version = "v1",
                        Title = "Jesters Credit Union API"
                    });
                o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                o.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                        },
                        Array.Empty<string>()
                    }
                });
            });
            builder.Services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddAuthentication(builder.Configuration)
            .AddGoogleAuthentication(builder.Configuration);
            builder.Services.AddSingleton<IAuthorizationHandler, AuthorizationHandler>();
            builder.Services.AddAuthorization(builder.Configuration);

            WebApplication app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseRouting();
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapControllers());

            app.Run();
        }
    }
}