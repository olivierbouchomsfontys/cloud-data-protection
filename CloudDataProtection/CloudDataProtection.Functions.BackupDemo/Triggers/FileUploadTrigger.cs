using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using CloudDataProtection.Core.Result;
using CloudDataProtection.Functions.BackupDemo.Business;
using CloudDataProtection.Functions.BackupDemo.Entities;
using CloudDataProtection.Functions.BackupDemo.Triggers.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CloudDataProtection.Functions.BackupDemo.Triggers
{
    public static class FileUploadTrigger
    {
        [FunctionName("FileUpload")]
        public static async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]
            HttpRequest request, ILogger logger)
        {
            IFormFile file = request.Form.Files.FirstOrDefault();

            if (file == null)
            { 
                return new BadRequestResult();
            }
            
            logger.LogInformation("Received file: {File}", JsonConvert.SerializeObject(file, Formatting.Indented));

            return await DoFileUpload(file);
        }

        private static async Task<IActionResult> DoFileUpload(IFormFile file)
        {
            FileUploadBusinessLogic logic = new FileUploadBusinessLogic();

            BusinessResult<File> result = await logic.Upload(file);

            if (!result.Success || result.Data == null)
            {
                return new InternalServerErrorResult();
            }

            FileUploadResult dto = new FileUploadResult
            {
                StorageId = result.Data.StorageId
            };
            
            return new OkObjectResult(dto);
        }
    }
}