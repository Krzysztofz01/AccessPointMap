using AccessPointMap.Application.Oui.Core;
using AccessPointMap.Application.Pcap.Core;
using AccessPointMap.Infrastructure.Core.Abstraction;
using System;

namespace AccessPointMap.Application.Integration.Core
{
    public abstract class AccessPointIntegrationBase<TIntegration> where TIntegration : AccessPointIntegrationBase<TIntegration>
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IScopeWrapperService _scopeWrapperService;
        protected readonly IPcapParsingService _pcapParsingService;
        protected readonly IOuiLookupService _ouiLookupService;

        protected abstract string IntegrationName { get; }
        protected abstract string IntegrationDescription { get; }
        protected abstract string IntegrationVersion { get; }

        private AccessPointIntegrationBase() { }

        public AccessPointIntegrationBase(
            IUnitOfWork unitOfWork,
            IScopeWrapperService scopeWrapperService,
            IPcapParsingService pcapParsingService,
            IOuiLookupService ouiLookupService)
        {
            _unitOfWork = unitOfWork ??
                throw new ArgumentNullException(nameof(unitOfWork));

            _scopeWrapperService = scopeWrapperService ??
                throw new ArgumentNullException(nameof(scopeWrapperService));

            _pcapParsingService = pcapParsingService ??
                throw new ArgumentNullException(nameof(pcapParsingService));

            _ouiLookupService = ouiLookupService ??
                throw new ArgumentNullException(nameof(ouiLookupService));
        }

        public override string ToString() =>
            $"{IntegrationName} - {IntegrationVersion}";
    }
}
