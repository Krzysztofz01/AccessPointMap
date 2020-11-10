using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using ms_accesspointmap_api.Models;
using ms_accesspointmap_api.Repositories;
using ms_accesspointmap_api.Services;
using ms_accesspointmap_api.Settings;

namespace ms_accesspointmap_api
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
            //Database 
            services.AddDbContext<AccessPointMapContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("AccessPointMap")));

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

            //Cross-Origin Resource Sharing
            services.AddCors(o => o.AddPolicy("DefaultPolicy", builder =>
            {
                builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
            }));

            //Repositories
            services.AddScoped<IAccessPointsRepository, AccessPointsRepository>();
            services.AddScoped<IBrandRepository, BrandRepository>();
            services.AddScoped<IGuestAccesspointsRepository, GuestAccesspointsRepository>();
            services.AddScoped<IUserRepository, UsersRepository>();

            //Services
            services.AddTransient<IAuthenticationService, AuthenticationService>();

            //Constrollers
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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
        }
    }
}
