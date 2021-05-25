using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CloudDataProtection.Core.Environment;

namespace CloudDataProtection.Ocelot
{
    public abstract class BackupDemoFileHandlerBase : DelegatingHandler
    {
        private readonly string _functionsKey;

        protected BackupDemoFileHandlerBase()
        {
            _functionsKey = EnvironmentVariableHelper.GetEnvironmentVariable("CDP_BACKUP_DEMO_FUNCTIONS_KEY");
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Add("x-functions-key", _functionsKey);
            
            return base.SendAsync(request, cancellationToken);
        }
    }
}