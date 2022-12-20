using AccessPointMap.Application.Oui.Core;
using AccessPointMap.Application.Pcap.Core;
using AccessPointMap.Infrastructure.Core.Abstraction;
using Microsoft.Extensions.Logging;
using System;

namespace AccessPointMap.Application.Integration.Core
{
    public abstract class AccessPointIntegrationBase<TIntegration> where TIntegration : AccessPointIntegrationBase<TIntegration>
    {
        protected readonly IUnitOfWork UnitOfWork;
        protected readonly IScopeWrapperService ScopeWrapperService;
        protected readonly IPcapParsingService PcapParsingService;
        protected readonly IOuiLookupService OuiLookupService;
        protected readonly ILogger<TIntegration> Logger;

        protected abstract string IntegrationName { get; }
        protected abstract string IntegrationDescription { get; }
        protected abstract string IntegrationVersion { get; }

        private AccessPointIntegrationBase() { }

        public AccessPointIntegrationBase(
            IUnitOfWork unitOfWork,
            IScopeWrapperService scopeWrapperService,
            IPcapParsingService pcapParsingService,
            IOuiLookupService ouiLookupService,
            ILogger<TIntegration> logger)
        {
            UnitOfWork = unitOfWork ??
                throw new ArgumentNullException(nameof(unitOfWork));

            ScopeWrapperService = scopeWrapperService ??
                throw new ArgumentNullException(nameof(scopeWrapperService));

            PcapParsingService = pcapParsingService ??
                throw new ArgumentNullException(nameof(pcapParsingService));

            OuiLookupService = ouiLookupService ??
                throw new ArgumentNullException(nameof(ouiLookupService));

            Logger = logger ??
                throw new ArgumentNullException(nameof(logger));
        }

        public override string ToString() =>
            $"{IntegrationName} - {IntegrationVersion}";
    }
}
