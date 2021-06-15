using AccessPointMap.Repository;
using AccessPointMap.Repository.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

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

            //Cross-Origin Resource Sharing
            services.AddCors(o => o.AddPolicy("DefaultPolicy", builder =>
            {
                builder.AllowAnyHeader().AllowAnyMethod().SetIsOriginAllowed(o => true).AllowCredentials();
            }));

            //JWT Authentication

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
