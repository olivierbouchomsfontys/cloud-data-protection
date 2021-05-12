using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CloudDataProtection.Functions.BackupDemo.Triggers
{
    public static class FileUploadTrigger
    {
        [FunctionName("FileUpload")]
        public static async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]
            HttpRequest request, ILogger logger)
        {
            IFormFile file = request.Form.Files.FirstOrDefault();

            if (file == null)
            { 
                return new BadRequestResult();
            }
            
            logger.LogInformation("Received file: {File}", JsonConvert.SerializeObject(file, Formatting.Indented));

            return new OkResult();
        }
    }
}