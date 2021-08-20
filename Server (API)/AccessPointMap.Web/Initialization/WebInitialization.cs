using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace AccessPointMap.Web.Initialization
{
    public static class WebInitialization
    {
        public static IServiceCollection AddWebServices(this IServiceCollection services)
        {
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

            //Http Client Factory
            services.AddHttpClient();

            //Controllers
            services.AddControllers();

            return services;
        }
    }
}
