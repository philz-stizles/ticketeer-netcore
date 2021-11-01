using Amazon.Runtime;
using Amazon.S3;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
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

namespace Ticketeer.WebApp
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                 .SetBasePath(env.ContentRootPath)
                 .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                 .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsEnvironment("Development"))
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
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
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
            services.AddScoped<IPasswordService, PasswordService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<ITicketRepository, TicketRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddRazorPages();
            services.AddServerSideBlazor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
