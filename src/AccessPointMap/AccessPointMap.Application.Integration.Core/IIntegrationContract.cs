using System.Threading.Tasks;

namespace AccessPointMap.Application.Integration.Core
{
    public interface IIntegrationContract
    {
        public Task Handle(IIntegrationCommand command);
    }
}
