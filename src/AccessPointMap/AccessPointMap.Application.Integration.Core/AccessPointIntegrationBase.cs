using AccessPointMap.Infrastructure.Core.Abstraction;
using System;

namespace AccessPointMap.Application.Integration.Core
{
    public abstract class AccessPointIntegrationBase<TIntegration> where TIntegration : AccessPointIntegrationBase<TIntegration>
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IScopeWrapperService _scopeWrapperService;

        protected abstract string IntegrationName { get; }
        protected abstract string IntegrationDescription { get; }
        protected abstract string IntegrationVersion { get; }

        private AccessPointIntegrationBase() { }

        public AccessPointIntegrationBase(IUnitOfWork unitOfWork, IScopeWrapperService scopeWrapperService)
        {
            _unitOfWork = unitOfWork ??
                throw new ArgumentNullException(nameof(unitOfWork));

            _scopeWrapperService = scopeWrapperService ??
                throw new ArgumentNullException(nameof(scopeWrapperService));
        }

        public override string ToString() =>
            $"{IntegrationName} - {IntegrationVersion}";
    }
}
