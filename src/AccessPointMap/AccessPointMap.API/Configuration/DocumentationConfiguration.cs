using AccessPointMap.Application.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System;

namespace AccessPointMap.API.Configuration
{
    internal static class DocumentationConfiguration
    {
        public static IServiceCollection AddDocumentation(this IServiceCollection services, IConfiguration configuration)
        {
            var swaggerSettingsSection = configuration.GetSection(nameof(SwaggerSettings));
            services.Configure<SwaggerSettings>(swaggerSettingsSection);

            var swaggerSettings = swaggerSettingsSection.Get<SwaggerSettings>();
            if (swaggerSettings.Enabled)
            {
                services.AddSwaggerGen(options =>
                {
                    var apiInfo = new OpenApiInfo
                    {
                        Version = "v1",
                        Title = "AccessPointMap Web API"
                    };

                    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer",
                        BearerFormat = "JWT"
                    });

                    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                            Array.Empty<string>()
                        }
                    });

                    options.SwaggerDoc("v1", apiInfo);
                    options.CustomSchemaIds(type => type.FullName.Replace('+', '_'));
                });
            }

            return services;
        }

        public static IApplicationBuilder UseDocumentation(this IApplicationBuilder app, IServiceProvider service)
        {
            var swaggerSettings = service.GetService<IOptions<SwaggerSettings>>().Value ??
                throw new InvalidOperationException("Swagger settubgs are not available.");

            if (swaggerSettings.Enabled)
            {
                app.UseSwagger();

                app.UseSwaggerUI(options =>
                {
                    options.RoutePrefix = "api/swagger";
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                });
            }

            return app;
        }
    }
}
