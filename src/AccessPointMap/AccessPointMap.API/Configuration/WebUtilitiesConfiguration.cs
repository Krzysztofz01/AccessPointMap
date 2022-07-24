using AccessPointMap.API.Converters;
using AccessPointMap.API.Middleware;
using AccessPointMap.Application.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
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

                    var securityDefinition = new OpenApiSecurityScheme
                    {
                        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer",
                        BearerFormat = "JWT"
                    };

                    var securityRequirements = new OpenApiSecurityRequirement
                    {
                        { securityDefinition, Array.Empty<string>() }
                    };

                    options.SwaggerDoc("v1", apiInfo);
                    options.CustomSchemaIds(type => type.FullName.Replace('+', '_'));
                    options.AddSecurityDefinition("Bearer", securityDefinition);
                    options.AddSecurityRequirement(securityRequirements);
                });
            }

            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes;
                options.Providers.Add<GzipCompressionProvider>();
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

            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new AntiXssConverter());
            });

            return services;
        }

        public static IApplicationBuilder UseWebUtilities(this IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider service)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

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

            var securitySettings = service.GetService<IOptions<SecuritySettings>>().Value ??
                throw new InvalidOperationException("Security settings are not available.");
            
            if (securitySettings.SecureMode)
            {
                app.UseHsts();

                app.UseHttpsRedirection();
            }

            app.UseResponseCompression();

            app.UseMiddleware<ExceptionMiddleware>();

            // TODO: AnitXssMiddleware requires a fix. Form files are causing false-positives.
            // A custom serializer rule is used for string validation instead of this middleware
            // app.UseMiddleware<AntiXssMiddleware>();

            app.UseRouting();

            app.UseCors(securitySettings.SecureMode ? _secureCorsPolicyName : _defaultCorsPolicyName);

            return app;
        }
    }
}
