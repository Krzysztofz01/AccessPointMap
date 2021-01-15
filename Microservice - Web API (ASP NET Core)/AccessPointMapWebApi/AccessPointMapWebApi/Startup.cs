using AccessPointMapWebApi.DatabaseContext;
using AccessPointMapWebApi.Repositories;
using AccessPointMapWebApi.Services;
using AccessPointMapWebApi.Settings;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace AccessPointMapWebApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //Settings
            services.Configure<LogsDatabaseSettings>(Configuration.GetSection(nameof(LogsDatabaseSettings)));

            //Databases 
            services.AddDbContext<AccessPointMapContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("AccessPointMap")));
            services.AddSingleton<ILogsDatabaseSettings>(opt => opt.GetRequiredService<IOptions<LogsDatabaseSettings>>().Value);

            //Repositories
            services.AddScoped<IAccessPointRepository, AccessPointRepository>();
            services.AddScoped<IBrandRepository, BrandRepository>();
            services.AddScoped<IGuestAccesspointRepository, GuestAccesspointRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ILogsRepository, LogsRepository>();

            //Services
            services.AddTransient<IAuthenticationService, AuthenticationService>();
            services.AddTransient<IGeocalculationService, GeocalculationService>();
            services.AddTransient<IBrandUpdateService, BrandUpdateService>();
            services.AddTransient<ILogCleanupService, LogCleanupService>();

            //Cross-Origin Resource Sharing
            services.AddCors(o => o.AddPolicy("DefaultPolicy", builder =>
            {
                builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
            }));

            //Hangfire Server
            services.AddHangfire(config => config
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseDefaultTypeSerializer()
                .UseMemoryStorage()
            );

            services.AddHangfireServer();

            //JWT Authentication
            var JsonWebTokenSection = Configuration.GetSection("JsonWebTokenSettings");
            services.Configure<JsonWebTokenSettings>(JsonWebTokenSection);

            var JsonWebTokenConfig = JsonWebTokenSection.Get<JsonWebTokenSettings>();
            var key = Encoding.ASCII.GetBytes(JsonWebTokenConfig.Secret);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            //Http Client
            services.AddHttpClient();

            //Controllers
            services.AddControllers();
        }

        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            IRecurringJobManager recurringJobManager,
            IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("DefaultPolicy");

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //Hangfire
            app.UseHangfireServer();

            recurringJobManager.AddOrUpdate(
                "JOB_BRAND_UPDATE",
                () => serviceProvider.GetService<IBrandUpdateService>().Update(),
                Cron.Daily,
                TimeZoneInfo.Local);

            recurringJobManager.AddOrUpdate(
                "JOB_LOG_CLEAN",
                () => serviceProvider.GetService<ILogCleanupService>().Cleanup(),
                Cron.Daily,
                TimeZoneInfo.Local);
        }
    }
}
