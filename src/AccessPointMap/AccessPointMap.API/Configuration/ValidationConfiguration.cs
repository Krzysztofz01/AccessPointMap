using AccessPointMap.Application.Core.Abstraction;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace AccessPointMap.API.Configuration
{
    public static class ValidationConfiguration
    {
        public static IServiceCollection AddValidation(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation();

            services.AddValidatorsFromAssemblyContaining<ICommand>();

            ValidatorOptions.Global.LanguageManager.Enabled = false;

            return services;
        }
    }
}
