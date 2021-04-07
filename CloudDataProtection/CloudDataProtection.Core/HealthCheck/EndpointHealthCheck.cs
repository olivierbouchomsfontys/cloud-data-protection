using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CloudDataProtection.Core.HealthCheck
{
    public class EndpointHealthCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult(HealthCheckResult.Healthy());
        }
    }
}