﻿using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.DI.Core;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using MMSL.Actors;
using MMSL.Actors.Infrastructure;
using MMSL.Common;
using MMSL.Common.Exceptions.GlobalHandler;
using MMSL.Common.Exceptions.GlobalHandler.Contracts;
using MMSL.Common.IdentityConfiguration;
using MMSL.Common.Localization;
using MMSL.Common.ResponseBuilder;
using MMSL.Common.ResponseBuilder.Contracts;
using MMSL.Databases;
using MMSL.Domain.DataSourceAdapters.SQL;
using MMSL.Domain.DataSourceAdapters.SQL.Contracts;
using MMSL.Domain.DbConnectionFactory;
using MMSL.Domain.Repositories.Identity;
using MMSL.Domain.Repositories.Identity.Contracts;
using MMSL.Server.Core.Localization;
using MMSL.Services.IdentityServices;
using MMSL.Services.IdentityServices.Contracts;
using Newtonsoft.Json.Serialization;
using Serilog;
using Serilog.Formatting.Json;
using IApplicationLifetime = Microsoft.AspNetCore.Hosting.IApplicationLifetime;
using IWebHostEnvironment = Microsoft.AspNetCore.Hosting.IWebHostEnvironment;
namespace MMSL.Server.Core
{
    public class Startup
    {
        public IConfiguration Configuration { get; private set; }

        public IContainer ApplicationContainer { get; private set; }

        private readonly IWebHostEnvironment _environment;

        private ActorSystem _actorSystem;

        public Startup(IWebHostEnvironment env)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath);

            env.EnvironmentName = ProductEnvironment.Development;

            builder.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);
            builder.AddEnvironmentVariables();

            Configuration = builder.Build();

            ConfigurationManager.SetAppSettingsProperties(Configuration);

            ConfigurationManager.SetContentRootDirectoryPath(env.ContentRootPath);

            _environment = env;
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            ConfigurateDbConext(services);

            services.AddResponseCompression();

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    t => t.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });

            services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Fastest);

            ConfigureJwtAuthService(services);

            services.AddLocalization(options => { options.ResourcesPath = "Resources"; });

            services.AddMvc(options =>
            {
                options.CacheProfiles.Add(CacheControlProfiles.Default,
                    new CacheProfile()
                    {
                        Duration = 60
                    });
                options.CacheProfiles.Add(CacheControlProfiles.TwoHours,
                    new CacheProfile()
                    {
                        Duration = 7200
                    });
                options.CacheProfiles.Add(CacheControlProfiles.HalfDay,
                    new CacheProfile()
                    {
                        Duration = 43200
                    });
            });

            services.AddSignalR();

            services.Add(new ServiceDescriptor(typeof(ISqlDbContext),
                t => new SqlDbContext(new MMSLDbContext(t.GetService<DbContextOptions<MMSLDbContext>>())), ServiceLifetime.Transient)
            );

            ContainerBuilder builder = new ContainerBuilder();

            builder.Populate(services);

            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().SingleInstance();

            builder.RegisterType<GlobalExceptionHandler>().As<IGlobalExceptionHandler>();
            builder.RegisterType<GlobalExceptionFactory>().As<IGlobalExceptionFactory>();

            builder.RegisterType<ResponseFactory>().As<IResponseFactory>();
            builder.RegisterType<IdentityRepository>().As<IIdentityRepository>();
            builder.RegisterType<IdentityRolesRepository>().As<IIdentityRolesRepository>();
            builder.RegisterType<IdentityRepositoriesFactory>().As<IIdentityRepositoriesFactory>();


            builder.RegisterType<UserIdentityService>().As<IUserIdentityService>();
            builder.RegisterType<AccountService>().As<IAccountService>();

            builder.RegisterType<DbConnectionFactory>().As<IDbConnectionFactory>();
            builder.RegisterType<SqlDbContext>().As<ISqlDbContext>();
            builder.RegisterType<SqlContextFactory>().As<ISqlContextFactory>();

            builder.RegisterType<MasterActor>();

            ApplicationContainer = builder.Build();

            //DbInitializer.Initialize(context, env);

            _actorSystem = ActorSystem.Create("MainSystem");

            _actorSystem.UseAutofac(ApplicationContainer);

            IActorRef masterActor = _actorSystem.ActorOf(_actorSystem.DI().Props<MasterActor>(), ActorNames.MASTER_ACTOR);
            ActorReferenceManager.Instance.Add(ActorNames.MASTER_ACTOR, masterActor);

            return new AutofacServiceProvider(this.ApplicationContainer);
        }

        public void Configure(IApplicationBuilder app, MMSLDbContext ctx, IWebHostEnvironment env, IGlobalExceptionFactory globalExceptionFactory)
        {
            IHostApplicationLifetime appLifetime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();


            LoggerConfiguration logger = new LoggerConfiguration();
            logger.Enrich.FromLogContext().MinimumLevel.Information().WriteTo.File
                (new JsonFormatter(), "logs\\devInfo.json");

            Log.Logger = logger.CreateLogger();

            ConfigureLocalization(app);

            app.UseResponseCompression();

            app.UseCors();

            var webSocketOptions = new WebSocketOptions()
            {
                KeepAliveInterval = TimeSpan.FromSeconds(120),
                ReceiveBufferSize = 4 * 1024
            };
            app.UseWebSockets(webSocketOptions);

            app.UseDefaultFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                RequestPath = "/Exports",
                FileProvider = new PhysicalFileProvider(_environment.ContentRootPath + "\\exports")
            })
                .UseHttpsRedirection()
                .UseCookiePolicy()
                .UseAuthentication()
                .UseResponseCompression();

            app.UseAuthentication();

            app.UseExceptionHandler(builder =>
            {
                builder.Run(
                    async context =>
                    {
                        IExceptionHandlerFeature error = context.Features.Get<IExceptionHandlerFeature>();
                        IGlobalExceptionHandler globalExceptionHandler = globalExceptionFactory.New();

                        await globalExceptionHandler.HandleException(context, error, true);
                    });
            });

            appLifetime.ApplicationStopped.Register(Log.CloseAndFlush);
            appLifetime.ApplicationStopped.Register(() => { _actorSystem.Terminate().RunSynchronously(); });
        }

        private void ConfigurateDbConext(IServiceCollection services)
        {
            string connectionString;

            connectionString = Configuration.GetConnectionString(ConnectionStringNames.DefaultConnection);

            services.AddDbContext<MMSLDbContext>(options => options.UseSqlServer(connectionString));
            services.AddScoped(p => new MMSLDbContext(p.GetService<DbContextOptions<MMSLDbContext>>()));
        }

        private void ConfigureJwtAuthService(IServiceCollection services)
        {
            SymmetricSecurityKey signingKey = AuthOptions.GetSymmetricSecurityKey(ConfigurationManager.AppSettings.TokenSecret);

            TokenValidationParameters tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

                ValidateIssuer = true,
                ValidIssuer = AuthOptions.ISSUER,

                ValidateAudience = true,
                ValidAudience = AuthOptions.AUDIENCE_LOCAL,

                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = tokenValidationParameters;

                options.Events = new JwtBearerEvents()
                {
                    OnMessageReceived = context =>
                    {
                        StringValues accessToken;
                        if (context.Request.Query.ContainsKey("access_token"))
                        {
                            accessToken = context.Request.Query["access_token"];

                            context.Request.Headers.TryAdd("Authorization", $@"Bearer {accessToken}");
                        }
                        else
                        {
                            context.Request.Headers.TryGetValue("Authorization", out accessToken);
                        }
                        return Task.CompletedTask;
                    }
                };
            });
        }

        private void ConfigureLocalization(IApplicationBuilder app)
        {
            var localizationOptions = new RequestLocalizationOptions()
            {
                SupportedCultures = LocalizationConfigurations.SupportedCultures,
                SupportedUICultures = LocalizationConfigurations.SupportedCultures,
                DefaultRequestCulture = new RequestCulture(LocalizationConfigurations.DefaultCulture)
            };

            localizationOptions.RequestCultureProviders.Insert(0, new LocalizedRouteDataRequestCultureProvider(localizationOptions.DefaultRequestCulture));

            app.UseRequestLocalization(localizationOptions);
        }
    }
}
