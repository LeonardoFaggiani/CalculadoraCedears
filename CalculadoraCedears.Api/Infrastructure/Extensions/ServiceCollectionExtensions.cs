﻿using CalculadoraCedears.Api.CrossCutting.Bus;
using CalculadoraCedears.Api.CrossCutting.Jwt;
using CalculadoraCedears.Api.Infrastructure.BackgroundServices;
using CalculadoraCedears.Api.Infrastructure.Data;
using CalculadoraCedears.Api.Infrastructure.Exceptions.Builder;
using CalculadoraCedears.Api.Infrastructure.Extensions;
using CalculadoraCedears.Api.Infrastructure.Filters;
using CalculadoraCedears.Api.Infrastructure.HealthChecks;
using CalculadoraCedears.Api.Infrastructure.Repositories;
using CalculadoraCedears.Api.Infrastructure.Repositories.Base;
using CalculadoraCedears.Api.Infrastructure.WebSocket;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;

using NetDevPack.Mediator;

using System.Reflection;
using System.Text;

namespace CalculadoraCedears.Api.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {

        private const string SwaggerDocVersion = "v1";
        private static string AssemblyName => Assembly.GetEntryAssembly().GetName().Name;
        private static string ApiName => Assembly.GetExecutingAssembly().GetName().Name;

        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(opt =>
            {
                opt.DescribeAllParametersInCamelCase();

                opt.SwaggerDoc(SwaggerDocVersion, new OpenApiInfo()
                {
                    Title = AssemblyName,
                    Version = SwaggerDocVersion
                });

                string apiXmlFile = $"{AssemblyName}.xml";
                string apiXmlPath = Path.Combine(AppContext.BaseDirectory, apiXmlFile);
                opt.IncludeXmlComments(apiXmlPath);

                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    BearerFormat = "JWT",
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    In = ParameterLocation.Header,
                    Name = HeaderNames.Authorization,
                    Scheme = "Bearer",
                    Type = SecuritySchemeType.ApiKey
                });

                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
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

            return services;
        }

        public static IApplicationBuilder UseSwagger(this IApplicationBuilder app, IWebHostEnvironment environment)
        {
            app.UseSwagger();

            if (!environment.IsProduction())
            {
                app.UseSwaggerUI(opt =>
                {
                    string url = $"./swagger/{SwaggerDocVersion}/swagger.json";

                    string name = $"{ApiName} - {SwaggerDocVersion}";

                    opt.SwaggerEndpoint(url, name);
                    opt.RoutePrefix = string.Empty;

                    opt.EnableDeepLinking();
                });
            }

            return app;
        }


        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ICedearRepository, CedearRepository>();
            services.AddScoped<ICedearStockHoldingRepository, CedearStockHoldingRepository>();
            services.AddScoped<IBrokerRepository, BrokerRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IGoogleRepository, GoogleRepository>();

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            return services;
        }

        public static IServiceCollection AddBuilders(this IServiceCollection services)
        {
            services.AddTransient<IExceptionMessageBuilder, ExceptionMessageBuilder>();

            return services;
        }

        public static IServiceCollection AddConnectionString(this IServiceCollection services, IConfiguration configuration)
        {
            //In memory database used for simplicity, change to a real db for production applications
            //services.AddDbContext<CalculadoraCedearsContext>(options => { options.UseInMemoryDatabase("calculadoracedearsApiBD"); }, ServiceLifetime.Transient);
            services.AddDbContext<CalculadoraCedearsContext>(options => { options.UseSqlServer(configuration.GetSection("SQL:ConnectionStrings").Value); }, ServiceLifetime.Scoped);

            return services;
        }

        public static IServiceCollection AddAuthorization(this IServiceCollection services, IConfiguration configuration)
        {
            var secret = configuration.GetSection("JwtOptions:Secret").Value;

            services.AddMvcCore(opt =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();

                opt.Filters.Add(typeof(CustomExceptionFilter));

            })
            .AddApiExplorer()
            .AddDataAnnotations();

            services.AddCors();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.TokenValidationParameters = new TokenValidationParameters
                {                    
                    ValidateIssuer = true,
                    ValidIssuers = new[] { configuration.GetSection("JwtOptions:Issuer").Value },
                    ValidateAudience = true,
                    ValidAudience = configuration.GetSection("JwtOptions:Audience").Value,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))
                };               
            });

            return services;
        }

        public static IServiceCollection AddDomainEvents(this IServiceCollection services)
        {
            services.AddScoped<IMediatorHandler, InMemoryBus>();

            return services;
        }

        public static IServiceCollection AddBackgroundServices(this IServiceCollection services)
        {
            services.AddHostedService<UpdateCedearsPriceBackgroundService>();

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton<ICedearsStockHoldingUpdateService, CedearsStockHoldingUpdateService>();
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

            return services;
        }

        public static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtOptions>(configuration.GetSection("JwtOptions"));

            return services;
        }

        public static IApplicationBuilder UseHealthChecks(this IApplicationBuilder app)
        {
            app.UseHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = HealthCheckResponseWriter.Write
            });
            return app;
        }
    }
}