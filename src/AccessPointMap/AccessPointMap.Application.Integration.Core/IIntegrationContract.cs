using AccessPointMap.Application.Core;
using System.Threading;
using System.Threading.Tasks;

namespace AccessPointMap.Application.Integration.Core
{
    public interface IIntegrationContract
    {
        public Task<Result> HandleCommandAsync(IIntegrationCommand command, CancellationToken cancellationToken = default);
        public Task<Result<object>> HandleQueryAsync(IIntegrationQuery query, CancellationToken cancellationToken = default);
    }
}
