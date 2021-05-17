using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CloudDataProtection.Functions.BackupDemo.Service.Result;

namespace CloudDataProtection.Functions.BackupDemo.Service
{
    public interface IFileService
    {
        Task<UploadResult> Upload(Stream stream, string uploadFileName, IDictionary<string, string> tags);

        Task<InfoResult> GetInfo(string id);

        Task<Stream> Download(string id);
    }
}