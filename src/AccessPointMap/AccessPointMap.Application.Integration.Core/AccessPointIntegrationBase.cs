using AccessPointMap.Infrastructure.Core.Abstraction;
using Microsoft.Extensions.Logging;
using System;

namespace AccessPointMap.Application.Integration.Core
{
    public abstract class AccessPointIntegrationBase<TIntegration> where TIntegration : AccessPointIntegrationBase<TIntegration>
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly ILogger<TIntegration> _logger;

        protected abstract string IntegrationName { get; }
        protected abstract string IntegrationDescription { get; }
        protected abstract string IntegrationVersion { get; }

        private AccessPointIntegrationBase() { }

        public AccessPointIntegrationBase(IUnitOfWork unitOfWork, ILogger<TIntegration> logger)
        {
            _unitOfWork = unitOfWork ??
                throw new ArgumentNullException(nameof(unitOfWork));

            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
        }

        public override string ToString() =>
            $"{IntegrationName} - {IntegrationVersion}";
    }
}
