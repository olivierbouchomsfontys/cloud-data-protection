using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CloudDataProtection.Ocelot.Swagger
{
    public class OcelotControllerFilter : IDocumentFilter
    {
        private static readonly string[] IgnoredPaths =
        {
            "/configuration",
            "/outputcache/{region}"
        };

        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            foreach (string ignorePath in IgnoredPaths)
            {
                swaggerDoc.Paths.Remove(ignorePath);
            }
        }
    }
}