using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Hellang.Middleware.ProblemDetails;
using IogServices.BackgroundTasks;
using IogServices.Constants;
using IogServices.Enums;
using IogServices.ExceptionHandlers;
using IogServices.Models.DTO;
using IogServices.HubConfig;
using IogServices.HubConfig.Hubs;
using IogServices.Models.DAO;
using IogServices.Repositories;
using IogServices.Repositories.Impl;
using IogServices.Services;
using IogServices.Services.Impl;
using IogServices.Util;
using IogServices.Util.Impl;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MqttClientLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace IogServices
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static string ConnectionString { get; set; }
        public static IogHubInstances IogHubInstances = new IogHubInstances();

        private static readonly LoggerFactory MyLoggerFactory = new LoggerFactory(new[]
            {
                new ConsoleLoggerProvider(
                    (category, level)
                        => category == DbLoggerCategory.Database.Command.Name
                           && level == LogLevel.Critical, true)
            }
        );

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            ConnectionString = Configuration.GetConnectionString("IOG_DB_SERVER");

            
            // Options - START
            services.Configure<Middlewares>(Configuration.GetSection("Middlewares"));
            services.Configure<Forwarder>(Configuration.GetSection("Forwarder"));
            services.Configure<CommandRules>(Configuration.GetSection("CommandRules"));
            //Options - END

            
            // Service Configuration - START
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.DateFormatString = "yyyy-MM-ddTHH:mm:ssZ";
                    options.SerializerSettings.Formatting = Formatting.Indented;
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.AllowInputFormatterExceptionMessages = true;
                });

            services.ConfigureProblemDetailsModelState();

            services.CustomAddProblemDetails();
            services.AddSignalR(o => o.EnableDetailedErrors = true);

            services.AddScoped<IHubService, HubService>();

            services.AddScoped<IServicesUtils, ServicesUtils>();

            services.AddScoped<IManufacturerRepository, ManufacturerRepository>();
            services.AddScoped<IManufacturerService, ManufacturerService>();

            services.AddScoped<ISmcModelRepository, SmcModelRepository>();
            services.AddScoped<ISmcModelService, SmcModelService>();

            services.AddScoped<IMeterModelRepository, MeterModelRepository>();
            services.AddScoped<IMeterModelService, MeterModelService>();

            services.AddScoped<IRateTypeRepository, RateTypeRepository>();
            services.AddScoped<IRateTypeService, RateTypeService>();

            services.AddScoped<IMeterRepository, MeterRepository>();
            services.AddScoped<IMeterService, MeterService>();

            services.AddScoped<IMeterEnergyRepository, MeterEnergyRepository>();
            services.AddScoped<IMeterEnergyService, MeterEnergyService>();

            services.AddScoped<ISmcRepository, SmcRepository>();
            services.AddScoped<ISmcService, SmcService>();

            services.AddScoped<IModemRepository, ModemRepository>();
            services.AddScoped<IModemService, ModemService>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            
            services.AddScoped<IMeterKeyRepository, MeterKeyRepository>();
            services.AddScoped<IMeterKeyService, MeterKeyService>();

            services.AddScoped<ITicketRepository, TicketRepository>();
            services.AddScoped<ITicketService, TicketService>();

            services.AddScoped<ICommandTicketRepository, CommandTicketRepository>();
            services.AddScoped<ICommandService, CommandService>();

            services.AddScoped<IMiddlewareProviderService, MiddlewareProviderService>();
            
            services.AddScoped<ISmcForwarderService, SmcForwarderService>();
            services.AddScoped<IMeterForwarderService, MeterForwarderService>();

            services.AddScoped<IDeviceLogRepository, DeviceLogRepository>();
            services.AddScoped<IDeviceLogService, DeviceLogService>();
                        
            services.AddScoped<ISmcAlarmRepository, SmcAlarmRepository>();
            services.AddScoped<ISmcAlarmService, SmcAlarmService>();
            
            services.AddScoped<IMeterAlarmRepository, MeterAlarmReposirory>();
            services.AddScoped<IMeterAlarmService, MeterAlarmService>();
            
            services.AddScoped<ISmcUnregisteredRespository, SmcUnregisteredRepository>();
            services.AddScoped<ISmcUnregisteredService, SmcUnregisteredService>();

            services.AddScoped<IMeterUnregisteredRespository, MeterUnregisteredRepository>();
            services.AddScoped<IMeterUnregisteredService, MeterUnregisteredService>();
            
            services.AddScoped<IEletraMiddlewareService, EletraMiddlewareService>();
            services.AddScoped<INansenMiddlewareService, NansenMiddlewareService>();

            services.AddScoped<IForwarderSenderService, ForwarderSenderService>();

            services.AddSingleton<IEventService, EventService>();
            services.AddScoped<IEventHubService, EventHubService>();

            
            services.AddScoped<IThreadService, ThreadService>();
            services.AddSingleton<IMqttClientMethods, MqttClientMethods>();
            services.AddSingleton<IMqttClientConfiguration, MqttClientConfiguration>();
            services.AddScoped<IMiddlewaresMessageHandlerService, MiddlewaresMessageHandlerService>();
            
            
            services.AddHostedService<IogServicesMqttMessagesReceiverTask>();
            services.AddHostedService<IogServicesMqttCommandsReceiverHostedService>();
            services.AddHostedService<RestartCommandFieldsTask>();
            // Service Configuration - END


            // AutoMapper Configuration - START
            var mappingConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingProfile()); });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
            // AutoMapper Configuration - END


            // CORS Configuration - START
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.SetIsOriginAllowed((host) => true)
                        .AllowCredentials()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });
            // CORS Configuration - END


            // BD Configuration - START
            services.AddDbContext<IogContext>(options =>
                options
                    .UseLoggerFactory(MyLoggerFactory)
                    .UseSqlServer(Configuration.GetConnectionString("IOG_DB_SERVER")));
            // BD Configuration - END


            // Swagger Configuration - START
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "IoG API MDC Controller", Version = "v2"});

                var security = new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[] { }},
                };

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description =
                        "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
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
                            }
                        },
                        new string[] { }
                    }
                });
            });
            // Swagger Configuration - END
            
            
            // Token Configuration - START    
            var signingConfigurations = new SigningConfigurations();
            services.AddSingleton(signingConfigurations);
            
            var tokenConfigurations = new TokenConfigurations();
            new ConfigureFromConfigurationOptions<TokenConfigurations>(
                    Configuration.GetSection("TokenConfigurations"))
                .Configure(tokenConfigurations);
            services.AddSingleton(tokenConfigurations);
            
            services.AddAuthentication(authOptions =>
            {
                authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(bearerOptions =>
            {
                var paramsValidation = bearerOptions.TokenValidationParameters;
                paramsValidation.IssuerSigningKey = signingConfigurations.Key;
                paramsValidation.ValidAudience = tokenConfigurations.Audience;
                paramsValidation.ValidIssuer = tokenConfigurations.Issuer;
                paramsValidation.ValidateIssuerSigningKey = true;
                paramsValidation.ValidateLifetime = true;
                paramsValidation.ClockSkew = TimeSpan.Zero;
            });

            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser().Build());
            });
            // Token Configuration - END
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseProblemDetails().UseMvc();


            // CORS Configuration - START
            app.UseCors("CorsPolicy");
        
            // CORS Configuration - END


            //SignalR  Configuration - START
            app.UseSignalR(
                routes =>
                {
                    // routes.MapHub<MeterHub>("/meterHub",
                    //     options => { options.ApplicationMaxBufferSize = 3000 * 1024; });
                    // routes.MapHub<SmcHub>("/smcHub",
                    //     options => { options.ApplicationMaxBufferSize = 3000 * 1024; });
                    // routes.MapHub<EnergyHub>("/energyHub",
                    //     options => { options.ApplicationMaxBufferSize = 3000 * 1024; });
                    // routes.MapHub<MeterAlarmHub>("/alarmHub",
                    //     options => { options.ApplicationMaxBufferSize = 3000 * 1024; });
                    // routes.MapHub<DeviceLogHub>("/logHub",
                    //     options => { options.ApplicationMaxBufferSize = 3000 * 1024; });
                    routes.MapHub<GeneralHubObject>("/generalHub",
                        options => { options.ApplicationMaxBufferSize = 3000 * 1024; });
                    
                });
            
            // IogHubInstances.MeterHubContext = app.ApplicationServices.GetService<IHubContext<MeterHub>>();
            // IogHubInstances.SmcHubContext = app.ApplicationServices.GetService<IHubContext<SmcHub>>();
            // IogHubInstances.EnergyHubContext = app.ApplicationServices.GetService<IHubContext<EnergyHub>>();
            // IogHubInstances.MeterAlarmHubContext = app.ApplicationServices.GetService<IHubContext<MeterAlarmHub>>();
            IogHubInstances.GeneralHubContext = app.ApplicationServices.GetService<IHubContext<GeneralHubObject>>();
            
            //SignalR Configuration - END

            
            // Swagger Configuration - START
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "IoG API MDC Controller v2");
                c.RoutePrefix = "";
            });
            // Swagger Configuration - END


            // BD Configuration - START
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<IogContext>();
                context.Database.Migrate();
                
                if (!context.Users.Any())
                {
                    var salt = User.GenerateSalt();
                    var pass = User.GenerateHash("admin", salt);

                    context.Users.Add(new User
                    {
                        Description = "Usuário administrador",
                        Email = "admin@email.com",
                        Name = "admin",
                        Password = pass,
                        UserType = UserType.Admin,
                        Salt = Convert.ToBase64String(salt)
                    });
                    context.SaveChanges();
                }
            }
            // BD Configuration - END


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseMvc();
        }
    }
}