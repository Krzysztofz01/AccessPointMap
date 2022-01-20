using AccessPointMap.API.Middleware;
using AccessPointMap.Application.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System;
using System.Net;

namespace AccessPointMap.API.Configuration
{
    public static class WebUtilitiesConfiguration
    {
        public const string _defaultCorsPolicyName = "DefaultCorsPolicy";
        public const string _secureCorsPolicyName = "SecureCorsPolicy";

        public static IServiceCollection AddWebUtilities(this IServiceCollection services, IConfiguration configuration)
        {
            var securitySettingsSection = configuration.GetSection(nameof(SecuritySettings));
            services.Configure<SecuritySettings>(securitySettingsSection);

            var securitySettings = securitySettingsSection.Get<SecuritySettings>();
            if (securitySettings.SecureMode)
            {
                services.AddHttpsRedirection(options =>
                {
                    options.RedirectStatusCode = (int)HttpStatusCode.PermanentRedirect;
                    options.HttpsPort = 443;
                });
            }

            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
            });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "AccessPointMap Web API", Version = "v1" });
                options.CustomSchemaIds(type => type.ToString());
            });

            services.AddHttpContextAccessor();

            services.AddCors(options =>
            {
                options.AddPolicy(_defaultCorsPolicyName, builder =>
                {
                    builder
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .SetIsOriginAllowed(options => true)
                        .AllowCredentials();
                });

                if (securitySettings.SecureMode)
                {
                    options.AddPolicy(_secureCorsPolicyName, builder =>
                    {
                        builder
                            .AllowAnyHeader()
                            .WithMethods("PUT", "POST", "OPTIONS", "GET", "DELETE")
                            .WithOrigins(securitySettings.Origins)
                            .AllowCredentials();

                    });
                }
            });

            services.AddControllers();

            return services;
        }

        public static IApplicationBuilder UseWebUtilities(this IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider service)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();

                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "AccessPointMap Web API v1");
                });

                app.UseDeveloperExceptionPage();
            }

            var securitySettings = service.GetService<IOptions<SecuritySettings>>().Value ??
                throw new InvalidOperationException("Security settings are not available");
            
            if (securitySettings.SecureMode)
            {
                app.UseHsts();

                app.UseHttpsRedirection();
            }

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseRouting();

            app.UseCors(securitySettings.SecureMode ? _secureCorsPolicyName : _defaultCorsPolicyName);

            return app;
        }
    }
}
