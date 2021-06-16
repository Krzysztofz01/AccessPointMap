using AccessPointMap.Repository;
using AccessPointMap.Repository.Context;
using AccessPointMap.Service;
using AccessPointMap.Service.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Text;

namespace AccessPointMap.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //Settings
            services.Configure<AdminSettings>(Configuration.GetSection(nameof(AdminSettings)));

            //Automapper
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<Service.Profiles.UserProfile>();
                cfg.AddProfile<Service.Profiles.AccessPointProfile>();

                cfg.AddProfile<Web.Profiles.UserProfile>();
                cfg.AddProfile<Web.Profiles.AccessPointProfile>();
            });

            //Database
            services.AddDbContext<AccessPointMapDbContext>(opt =>
                opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnectionStringSql")));

            //Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAccessPointRepository, AccessPointRepository>();

            //Services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAccessPointService, AccessPointService>();

            services.AddScoped<IMacResolveService, MacResolveService>();
            services.AddScoped<IGeoCalculationService, GeoCalculationService>();

            //Cross-Origin Resource Sharing
            services.AddCors(o => o.AddPolicy("DefaultPolicy", builder =>
            {
                builder.AllowAnyHeader().AllowAnyMethod().SetIsOriginAllowed(o => true).AllowCredentials();
            }));

            //JWT Authentication
            var jwtSettingsSection = Configuration.GetSection(nameof(JWTSettings));
            services.Configure<JWTSettings>(jwtSettingsSection);

            var jwtSettings = jwtSettingsSection.Get<JWTSettings>();
            var secret = Encoding.ASCII.GetBytes(jwtSettings.Secret);

            services.AddAuthentication(cfg =>
            {
                cfg.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                cfg.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;
                cfg.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secret),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };
            });

            //Endpoint versioning
            services.AddApiVersioning(opt =>
            {
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.DefaultApiVersion = new ApiVersion(1, 0);
            });

            //Swagger
            services.AddSwaggerGen(cfg =>
            {
                cfg.SwaggerDoc("v1", new OpenApiInfo { Title = "AccessPointMap WebAPI", Version = "v1" });
            });

            //Hangfire

            //Http Client Factory
            services.AddHttpClient();

            //Controllers
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseCors("DefaultPolicy");

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseSwagger();

            app.UseSwaggerUI(cfg =>
            {
                cfg.SwaggerEndpoint("/swagger/v1/swagger.json", "AccessPointMap WebAPI v1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
