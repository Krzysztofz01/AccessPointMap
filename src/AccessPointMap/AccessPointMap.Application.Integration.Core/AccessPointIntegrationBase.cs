using AccessPointMap.Application.Oui.Core;
using AccessPointMap.Application.Pcap.Core;
using AccessPointMap.Infrastructure.Core.Abstraction;
using System;

namespace AccessPointMap.Application.Integration.Core
{
    public abstract class AccessPointIntegrationBase<TIntegration> where TIntegration : AccessPointIntegrationBase<TIntegration>
    {
        protected readonly IUnitOfWork UnitOfWork;
        protected readonly IDataAccess DataAccess;
        protected readonly IScopeWrapperService ScopeWrapperService;
        protected readonly IPcapParsingService PcapParsingService;
        protected readonly IOuiLookupService OuiLookupService;

        protected abstract string IntegrationName { get; }
        protected abstract string IntegrationDescription { get; }
        protected abstract string IntegrationVersion { get; }

        private AccessPointIntegrationBase() { }

        public AccessPointIntegrationBase(
            IUnitOfWork unitOfWork,
            IDataAccess dataAccess,
            IScopeWrapperService scopeWrapperService,
            IPcapParsingService pcapParsingService,
            IOuiLookupService ouiLookupService)
        {
            UnitOfWork = unitOfWork ??
                throw new ArgumentNullException(nameof(unitOfWork));

            DataAccess = dataAccess ??
                throw new ArgumentNullException(nameof(dataAccess));

            ScopeWrapperService = scopeWrapperService ??
                throw new ArgumentNullException(nameof(scopeWrapperService));

            PcapParsingService = pcapParsingService ??
                throw new ArgumentNullException(nameof(pcapParsingService));

            OuiLookupService = ouiLookupService ??
                throw new ArgumentNullException(nameof(ouiLookupService));
        }

        public override string ToString() =>
            $"{IntegrationName} - {IntegrationVersion}";
    }
}
