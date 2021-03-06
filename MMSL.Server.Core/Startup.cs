﻿using Akka.Actor;
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
using Microsoft.OpenApi.Models;
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
using MMSL.Domain.Repositories.Addresses;
using MMSL.Domain.Repositories.Addresses.Contracts;
using MMSL.Domain.Repositories.Dealer;
using MMSL.Domain.Repositories.Dealer.Contracts;
using MMSL.Domain.Repositories.DeliveryTimelines;
using MMSL.Domain.Repositories.DeliveryTimelines.Contracts;
using MMSL.Domain.Repositories.Fabrics;
using MMSL.Domain.Repositories.Fabrics.Contracts;
using MMSL.Domain.Repositories.Identity;
using MMSL.Domain.Repositories.Identity.Contracts;
using MMSL.Domain.Repositories.Measurements;
using MMSL.Domain.Repositories.Measurements.Contracts;
using MMSL.Domain.Repositories.Options;
using MMSL.Domain.Repositories.Options.Contracts;
using MMSL.Domain.Repositories.ProductRepositories;
using MMSL.Domain.Repositories.ProductRepositories.Contracts;
using MMSL.Domain.Repositories.Stores;
using MMSL.Domain.Repositories.Stores.Contracts;
using MMSL.Domain.Repositories.Types;
using MMSL.Domain.Repositories.Types.Contracts;
using MMSL.Server.Core.Import;
using MMSL.Server.Core.Localization;
using MMSL.Services.BankDetailsServices;
using MMSL.Services.DealerServices;
using MMSL.Services.DealerServices.Contracts;
using MMSL.Services.DeliveryTimelines;
using MMSL.Services.DeliveryTimelines.Contracts;
using MMSL.Services.FabricServices;
using MMSL.Services.FabricServices.Contracts;
using MMSL.Services.IdentityServices;
using MMSL.Services.IdentityServices.Contracts;
using MMSL.Services.MeasurementServices;
using MMSL.Services.MeasurementServices.Contracts;
using MMSL.Services.OptionServices;
using MMSL.Services.OptionServices.Contracts;
using MMSL.Services.ProductCategories;
using MMSL.Services.ProductCategories.Contracts;
using MMSL.Services.StoreCustomerServices;
using MMSL.Services.StoreCustomerServices.Contracts;
using MMSL.Services.StoreServices.Contracts;
using MMSL.Services.Types;
using MMSL.Services.Types.Contracts;
using Serilog;
using Serilog.Formatting.Json;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Threading.Tasks;
using IWebHostEnvironment = Microsoft.AspNetCore.Hosting.IWebHostEnvironment;

namespace MMSL.Server.Core {
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
                options.EnableEndpointRouting = false;
                
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

#if DEBUG
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MMSL API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        }, new string[] { }
                    }
                });
                //c.DocInclusionPredicate
            });
#endif

            ContainerBuilder builder = new ContainerBuilder();

            builder.Populate(services);

            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().SingleInstance();

            builder.RegisterType<GlobalExceptionHandler>().As<IGlobalExceptionHandler>();
            builder.RegisterType<GlobalExceptionFactory>().As<IGlobalExceptionFactory>();

            // Repository Factories.
            builder.RegisterType<ResponseFactory>().As<IResponseFactory>();
            builder.RegisterType<IdentityRepository>().As<IIdentityRepository>();
            builder.RegisterType<IdentityRolesRepository>().As<IIdentityRolesRepository>();
            builder.RegisterType<TypesRepositoriesFactory>().As<ITypesRepositoriesFactory>();
            builder.RegisterType<StoreRepositoriesFactory>().As<IStoreRepositoriesFactory>();
            builder.RegisterType<DealerRepositoriesFactory>().As<IDealerRepositoriesFactory>();
            builder.RegisterType<OptionRepositoriesFactory>().As<IOptionRepositoriesFactory>();
            builder.RegisterType<AddressRepositoriesFactory>().As<IAddressRepositoriesFactory>();
            builder.RegisterType<IdentityRepositoriesFactory>().As<IIdentityRepositoriesFactory>();
            builder.RegisterType<MeasurementsRepositoriesFactory>().As<IMeasurementsRepositoriesFactory>();
            builder.RegisterType<ProductCategoryRepositoriesFactory>().As<IProductCategoryRepositoriesFactory>();
            builder.RegisterType<DeliveryTimelineRepositoriesFactory>().As<IDeliveryTimelineRepositoriesFactory>();
            builder.RegisterType<FabricRepositoriesFactory>().As<IFabricRepositoriesFactory>();

            // Services.
            builder.RegisterType<StoreService>().As<IStoreService>();
            builder.RegisterType<OptionUnitService>().As<IOptionUnitService>();
            builder.RegisterType<OptionGroupService>().As<IOptionGroupService>();            
            builder.RegisterType<FittingTypeService>().As<IFittingTypeService>();
            builder.RegisterType<MeasurementService>().As<IMeasurementService>();
            builder.RegisterType<PaymentTypeService>().As<IPaymentTypeService>();
            builder.RegisterType<UserIdentityService>().As<IUserIdentityService>();
            builder.RegisterType<CurrencyTypeService>().As<ICurrencyTypeService>();
            builder.RegisterType<DealerAccountService>().As<IDealerAccountService>();
            builder.RegisterType<StoreCustomerService>().As<IStoreCustomerService>();
            builder.RegisterType<ProductCategoryService>().As<IProductCategoryService>();
            builder.RegisterType<MeasurementSizeService>().As<IMeasurementSizeService>();
            builder.RegisterType<MeasurementUnitService>().As<IMeasurementUnitService>();
            builder.RegisterType<DeliveryTimelineService>().As<IDeliveryTimelineService>();
            builder.RegisterType<MeasurementDefinitionService>().As<IMeasurementDefinitionService>();
            builder.RegisterType<ProductPermissionSettingService>().As<IProductPermissionSettingService>();
            builder.RegisterType<DealerProductAvailabilityService>().As<IDealerProductAvailabilityService>();
            builder.RegisterType<StoreCustomerProductProfileService>().As<IStoreCustomerProductProfileService>();
            builder.RegisterType<FabricService>().As<IFabricService>();

            builder.RegisterType<SqlDbContext>().As<ISqlDbContext>();
            builder.RegisterType<SqlContextFactory>().As<ISqlContextFactory>();
            builder.RegisterType<DbConnectionFactory>().As<IDbConnectionFactory>();

            builder.RegisterType<ExcelParser>();
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

#if DEBUG
            app.UseSwagger();

            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "MMSL API V1");
                c.RoutePrefix = string.Empty;
                c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
            });
#endif

            app.UseCors("CorsPolicy");

            app.UseRouting();

            ConfigureLocalization(app);

            app.UseResponseCompression();

            var webSocketOptions = new WebSocketOptions()
            {
                KeepAliveInterval = TimeSpan.FromSeconds(120),
                ReceiveBufferSize = 4 * 1024
            };
            app.UseWebSockets(webSocketOptions);

            app.UseDefaultFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                RequestPath = $"/{ConfigurationManager.UploadsDirectoryName}",
                FileProvider = new PhysicalFileProvider(_environment.ContentRootPath + $"\\{ConfigurationManager.UploadsDirectoryName}")
            })
                .UseHttpsRedirection()
                .UseCookiePolicy()
                .UseAuthentication()
                .UseResponseCompression();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

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
