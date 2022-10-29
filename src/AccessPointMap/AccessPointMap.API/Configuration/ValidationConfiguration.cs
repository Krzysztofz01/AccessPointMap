using AccessPointMap.Application.Authentication;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace AccessPointMap.API.Configuration
{
    internal static class ValidationConfiguration
    {
        public static IServiceCollection AddValidation(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation();

            services.AddValidatorsFromAssemblyContaining<IAuthenticationService>();

            ValidatorOptions.Global.LanguageManager.Enabled = false;

            return services;
        }
    }
}
