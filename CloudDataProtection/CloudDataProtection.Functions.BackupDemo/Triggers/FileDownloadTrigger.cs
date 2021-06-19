using System.Threading.Tasks;
using System.Web.Http;
using CloudDataProtection.Core.Result;
using CloudDataProtection.Functions.BackupDemo.Authentication;
using CloudDataProtection.Functions.BackupDemo.Business;
using CloudDataProtection.Functions.BackupDemo.Factory;
using CloudDataProtection.Functions.BackupDemo.Triggers.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace CloudDataProtection.Functions.BackupDemo.Triggers
{
    public static class FileDownloadTrigger
    {
        [FunctionName("FileDownload")]
        public static async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)]
            HttpRequest request, ILogger logger)
        {
            if (!request.HttpContext.IsAuthenticated())
            {
                return new UnauthorizedResult();
            }
            
            string id = request.Query["id"];
            
            if (string.IsNullOrWhiteSpace(id))
            {
                return new BadRequestResult();
            }
            
            logger.LogInformation("Received request for file download with id {Id}", id);

            if (request.HttpContext.Response.Headers.ContainsKey("Access-Control-Expose-Headers"))
            {
                request.HttpContext.Response.Headers["Access-Control-Expose-Headers"] += ", content-disposition";
            }
            else
            {
                request.HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", "content-disposition");
            }

            return await DoFileDownload(id);
        }

        private static async Task<IActionResult> DoFileDownload(string id)
        {
            FileManagerLogic logic = FileManagerLogicFactory.Instance.GetLogic();

            BusinessResult<FileDownloadResult> result = await logic.Download(id);

            if (!result.Success)
            {
                return new InternalServerErrorResult();
            }

            if (result.Data.Type == FileDownloadResultType.Bytes)
            {
                return new FileContentResult(result.Data.Bytes, result.Data.ContentType)
                {
                    FileDownloadName = result.Data.FileName
                };
            }

            return new OkResult();
        }
    }
}