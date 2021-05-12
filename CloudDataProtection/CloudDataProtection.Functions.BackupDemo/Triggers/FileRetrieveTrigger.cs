using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using CloudDataProtection.Core.Result;
using CloudDataProtection.Functions.BackupDemo.Business;
using CloudDataProtection.Functions.BackupDemo.Entities;
using CloudDataProtection.Functions.BackupDemo.Factory;
using CloudDataProtection.Functions.BackupDemo.Triggers.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace CloudDataProtection.Functions.BackupDemo.Triggers
{
    public static class FileRetrieveTrigger
    {
        [FunctionName("FileInfo")]
        public static async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
            HttpRequest request, ILogger logger)
        {
            string id = request.Query["id"];
            
            if (string.IsNullOrWhiteSpace(id))
            {
                return new BadRequestResult();
            }
            
            logger.LogInformation("Received request for file with id {Id}", id);

            return await DoGetFileInfo(id);
        }

        private static async Task<IActionResult> DoGetFileInfo(string id)
        {
            FileUploadBusinessLogic logic = FileUploadBusinessLogicFactory.Instance.GetLogic();

            BusinessResult<File> result = await logic.GetInfo(id);

            if (!result.Success)
            {
                return new InternalServerErrorResult();
            }

            FileInfoResult dto = new FileInfoResult
            {
                EncryptedName = result.Data.Name,
                Url = result.Data.Url,
                Bytes = result.Data.Bytes
            };
            
            return new OkObjectResult(dto);
        }
    }
}