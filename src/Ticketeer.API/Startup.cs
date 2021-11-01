using Amazon.Runtime;
using Amazon.S3;
using Hangfire;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies;
using Hangfire.Mongo.Migration.Strategies.Backup;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Ticketeer.API.Middlewares;
using Ticketeer.Application.Contracts;
using Ticketeer.Application.Contracts.Infrastructure.Repositories;
using Ticketeer.Application.Contracts.Infrastructure.Services;
using Ticketeer.Application.Contracts.Services;
using Ticketeer.Application.Contracts.Settings;
using Ticketeer.Application.Mappers;
using Ticketeer.Application.Services;
using Ticketeer.Application.Settings;
using Ticketeer.Infrastructure.Persistence;
using Ticketeer.Infrastructure.Persistence.Repositories;
using Ticketeer.Infrastructure.Services.Notifications;
using Ticketeer.Infrastructure.Services.Security;

namespace Ticketeer.API
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if(env.IsEnvironment("Development"))
            {
                // Read the configuration keys from the secret store.
                // Ensure generated user secret id = "dotnet user-secrets init"
                builder.AddUserSecrets<Program>();
            }

            builder.AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.Configure<JwtSettings>(Configuration.GetSection("JwtSettings"));
            services.Configure<CloudinarySettings>(Configuration.GetSection("CloudinarySettings"));
            services.Configure<SendGridSettings>(Configuration.GetSection("SendGridSettings"));
            services.Configure<MailgunSettings>(Configuration.GetSection("MailgunSettings"));
            services.Configure<FileSettings>(Configuration.GetSection("FileSettings"));
            services.Configure<AWSS3Settings>(Configuration.GetSection("AWSS3Settings"));
            services.Configure<AWSSESSettings>(Configuration.GetSection("AWSSESSettings"));
            services.Configure<StripeSettings>(Configuration.GetSection("StripeSettings"));
            services.Configure<TwilioSettings>(Configuration.GetSection("TwilioSettings"));

            services.AddAutoMapper(typeof(UserProfile).Assembly);
            services.Configure<MongoDbSettings>(Configuration.GetSection(nameof(MongoDbSettings)));

            services.AddSingleton<IMonogDbSettings>(sp =>
                sp.GetRequiredService<IOptions<MongoDbSettings>>().Value);

            // AWS S3 Bucket
            var awsOption = Configuration.GetAWSOptions();
            awsOption.Credentials = new BasicAWSCredentials(
                Configuration.GetValue<string>("AWSS3Settings:AccessKeyId"),
                Configuration.GetValue<string>("AWSS3Settings:SecretAccessKey")
            );

            services.AddDefaultAWSOptions(awsOption);
            services.AddAWSService<IAmazonS3>();
            // services.AddScoped<IAWSS3Service, AWSS3Service>();
            services.Configure<AWSS3Settings>(Configuration.GetSection(nameof(AWSS3Settings)));


            // AWS SES
            services.AddTransient<IEmailService, AWSSESService>();
            services.Configure<AWSSESSettings>(Configuration.GetSection(nameof(AWSSESSettings)));

            // AWS SNS
            // services.Configure<AWSSNSSettings>(Configuration.GetSection("AWSSNSSettings"));

            services.AddScoped<IDataDbContext, DataDbContext>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITicketService, TicketService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IPasswordService, PasswordService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<ITicketRepository, TicketRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddControllers(options =>
            {
                // options.Filters.Add<ExceptionFilter>();
            }).AddJsonOptions(x =>
            {
                // serialize enums as strings in api responses (e.g. Role)
                x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .SelectMany(x => x.Value.Errors)
                        .Select(x => x.ErrorMessage)
                        .ToArray();

                    var errorResponse = new
                    {
                        Status = false,
                        Errors = errors
                    };

                    return new BadRequestObjectResult(errorResponse);
                };
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Ticketeer.API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"bearer {token}\""
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                        new List<string>()
                        // new string[] {}

                    }
                });

                c.CustomSchemaIds(type => type.ToString());
            });
            services.AddHealthChecks()
                    .AddMongoDb(Configuration["MongoDbSettings:ConnectionString"], "MongoDb Health", HealthStatus.Degraded);

            // Add Hangfire services.
            var mongoUrlBuilder = new MongoUrlBuilder(Configuration.GetValue<string>("MongoDbSettings:JobsUrl"));
            var mongoClient = new MongoClient(mongoUrlBuilder.ToMongoUrl());
            services.AddHangfire(config => config
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseMongoStorage(mongoClient, mongoUrlBuilder.DatabaseName, new MongoStorageOptions
                {
                    MigrationOptions = new MongoMigrationOptions
                    {
                        MigrationStrategy = new MigrateMongoMigrationStrategy(),
                        BackupStrategy = new CollectionMongoBackupStrategy()
                    },
                    Prefix = "hangfire.mongo",
                    CheckConnection = true
                }));
            // Add the processing server as IHostedService
            services.AddHangfireServer(serverOptions =>
            {
                serverOptions.ServerName = "Hangfire.Mongo server 1";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ticketeer.API v1"));
            }

            // Global exception handling
            app.UseMiddleware<GlobalExceptionMiddleware>();

            app.UseRouting();

            // custom auth middleware.
            app.UseMiddleware<AuthMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/healthcheck", new HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
            });

            #region Configure Hangfire  
            //Basic Authentication added to access the Hangfire Dashboard  
            app.UseHangfireDashboard("/hangfire", new DashboardOptions()
            {
                AppPath = null,
                DashboardTitle = "Hangfire Dashboard",
                /* Authorization = new[]{
                     new HangfireCustomBasicAuthenticationFilter{
                         User = configuration.GetSection("HangfireCredentials:UserName").Value,
                         Pass = configuration.GetSection("HangfireCredentials:Password").Value
                     }
                 },*/
                //Authorization = new[] { new DashboardNoAuthorizationFilter() },  
                //IgnoreAntiforgeryToken = true  
            });
            #endregion
        }
    }
}
