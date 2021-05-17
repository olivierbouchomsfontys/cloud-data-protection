using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CloudDataProtection.Core.Cryptography.Aes;
using CloudDataProtection.Core.Result;
using CloudDataProtection.Functions.BackupDemo.Service;
using CloudDataProtection.Functions.BackupDemo.Service.Result;
using CloudDataProtection.Functions.BackupDemo.Triggers.Dto;
using Microsoft.AspNetCore.Http;
using File = CloudDataProtection.Functions.BackupDemo.Entities.File;

namespace CloudDataProtection.Functions.BackupDemo.Business
{
    public class FileManagerLogic
    {
        private readonly IDataTransformer _transformer;
        private readonly ITransformer _stringTransformer;

        private readonly IFileService _fileService;

        private const int FilenameHashWorkFactor = 4;

        private const string FileNameKey = "original_name";
        private const string ContentTypeKey = "content_type";

        public FileManagerLogic(IFileService fileService, IDataTransformer transformer, ITransformer stringTransformer)
        {
            _fileService = fileService;
            _transformer = transformer;
            _stringTransformer = stringTransformer;
        }

        public async Task<BusinessResult<File>> Upload(IFormFile input)
        {
            string blobName = GetBlobName(input);
            
            IDictionary<string, string> tags = new Dictionary<string, string>();

            tags.Add(FileNameKey, _stringTransformer.Encrypt(input.FileName));
            tags.Add(ContentTypeKey, _stringTransformer.Encrypt(input.ContentType));

            using (Stream stream = _transformer.Encrypt(input.OpenReadStream()))
            {
                UploadResult result = await _fileService.Upload(stream, blobName, tags);

                File file = new File
                {
                    StorageId = result.Id
                };

                if (!result.Success)
                {
                    return BusinessResult<File>.Error("An unknown error occured while processing the file.");
                }

                return BusinessResult<File>.Ok(file);
            }      
        }

        public async Task<BusinessResult<File>> GetInfo(string id)
        {
            InfoResult info = await _fileService.GetInfo(id);

            if (!info.Success)
            {
                if (info.IsNotFoundError)
                {
                    return BusinessResult<File>.Error("Not found");
                }
                
                return BusinessResult<File>.Error("An unknown error occured while retrieving info of the file");
            }

            string originalFileName = _stringTransformer.Decrypt(info.Tags[FileNameKey]);
            string contentType = _stringTransformer.Decrypt(info.Tags[ContentTypeKey]);

            File file = new File
            {
                StorageId = id,
                Bytes = info.Bytes,
                Name = originalFileName,
                ContentType = contentType
            };
            
            return BusinessResult<File>.Ok(file);
        }

        public async Task<BusinessResult<FileDownloadResult>> Download(string id, bool decrypt)
        {
            BusinessResult<File> info = await GetInfo(id);

            if (!info.Success)
            {
                return BusinessResult<FileDownloadResult>.Error("An unknown error occured while retrieving info of the file");
            }
            
            if (decrypt)
            {
                return await DownloadAndDecrypt(id, info.Data.ContentType, info.Data.Name);
            }

            return await DownloadRaw(info.Data.Name);
        }

        private async Task<BusinessResult<FileDownloadResult>> DownloadAndDecrypt(string id, string contentType, string fileName)
        {
            Stream response = await _fileService.Download(id);
            
            byte[] decrypted = _transformer.Decrypt(response);

            if (decrypted == null || decrypted.Length == 0)
            {
                return BusinessResult<FileDownloadResult>.Error("An unknown error occured while attempting to download the file");
            }

            FileDownloadResult result = new FileDownloadResult
            {
                Bytes = decrypted,
                Type = FileDownloadResultType.Bytes,
                FileName = fileName,
                ContentType = contentType
            };
            
            return BusinessResult<FileDownloadResult>.Ok(result);
        }

        private async Task<BusinessResult<FileDownloadResult>> DownloadRaw(string fileName)
        {
            throw new NotImplementedException();
        }

        private string GetBlobName(IFormFile input)
        {
            string blobName = Guid.NewGuid() + "_" + input.FileName;

            string hash = BCrypt.Net.BCrypt.HashPassword(blobName, FilenameHashWorkFactor);

            char[] invalidChars = Path.GetInvalidFileNameChars().Append('.').ToArray();

            string[] sanitizedHash = hash.Split(invalidChars, StringSplitOptions.RemoveEmptyEntries);

            return string.Join("_", sanitizedHash);
        }
    }
}