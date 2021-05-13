using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using CloudDataProtection.Core.Cryptography.Aes;
using CloudDataProtection.Core.Environment;
using CloudDataProtection.Core.Result;
using CloudDataProtection.Functions.BackupDemo.Extensions;
using CloudDataProtection.Functions.BackupDemo.Triggers.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage.Blob;
using BlobProperties = Azure.Storage.Blobs.Models.BlobProperties;
using File = CloudDataProtection.Functions.BackupDemo.Entities.File;

namespace CloudDataProtection.Functions.BackupDemo.Business
{
    public class FileManagerLogic
    {
        private string ConnectionString => EnvironmentVariableHelper.GetEnvironmentVariable("CDP_DEMO_BLOB_CONNECTION");

        private readonly IFileTransformer _transformer;
        private readonly ITransformer _stringTransformer;

        private const int FilenameHashWorkFactor = 4;

        private const string FileNameKey = "original_name";

        public FileManagerLogic(IFileTransformer transformer, ITransformer stringTransformer)
        {
            _transformer = transformer;
            _stringTransformer = stringTransformer;
        }

        public async Task<BusinessResult<File>> Upload(IFormFile input)
        {
            BlobContainerClient client = await GetContainerClient();

            string blobName = GetBlobName(input);

            BlobClient blobClient = client.GetBlobClient(blobName);
            
            IDictionary<string, string> tags = new Dictionary<string, string>();

            tags.Add(FileNameKey, _stringTransformer.Encrypt(input.FileName));
            tags.Add("content_type", _stringTransformer.Encrypt(input.ContentType));

            Stream inputStream = input.OpenReadStream();
            
            using (Stream stream = _transformer.Encrypt(inputStream))
            {
                Response<BlobContentInfo> response = await blobClient.UploadAsync(stream);

                blobClient.SetTags(tags);

                File file = new File
                {
                    StorageId = blobName
                };

                if (response.IsSuccessStatusCode())
                {
                    return BusinessResult<File>.Ok(file);
                }

                return BusinessResult<File>.Error("An unknown error occured while processing the file.");
            }      
        }

        public async Task<BusinessResult<File>> GetInfo(string id)
        {
            BlobContainerClient client = await GetContainerClient();

            BlobClient blobClient = client.GetBlobClient(id);

            Response<BlobProperties> properties = blobClient.GetProperties();

            Response<GetBlobTagResult> tags = blobClient.GetTags();

            string originalFileName = _stringTransformer.Decrypt(tags.Value.Tags[FileNameKey]);

            if (properties.IsSuccessStatusCode() && tags.IsSuccessStatusCode())
            {
                File file = new File
                {
                    StorageId = id,
                    Bytes = (int) properties.Value.ContentLength,
                    Name = originalFileName
                };
                
                return BusinessResult<File>.Ok(file);
            }

            return BusinessResult<File>.Error("An unknown error occured while retrieving info of the file.");
        }

        public async Task<BusinessResult<FileDownloadResult>> Download(string id, bool decrypt)
        {
            BlobContainerClient client = await GetContainerClient();

            BlobClient blobClient = client.GetBlobClient(id);

            Response<GetBlobTagResult> tags = blobClient.GetTags();
            
            string originalFileName = _stringTransformer.Decrypt(tags.Value.Tags[FileNameKey]);
            string contentType = _stringTransformer.Decrypt(tags.Value.Tags["content_type"]);

            if (decrypt)
            {
                return await DownloadAndDecrypt(originalFileName, contentType, blobClient);
            }

            return await DownloadRaw(originalFileName, blobClient);
        }

        private async Task<BusinessResult<FileDownloadResult>> DownloadAndDecrypt(string fileName, string contentType, BlobClient client)
        {
            MemoryStream memoryStream = new MemoryStream();

            BlobDownloadInfo response = await client.DownloadAsync();
            
            // response.Content.CopyTo(memoryStream);

            byte[] decrypted = _transformer.Decrypt(response.Content);

            if (decrypted == null || decrypted.Length == 0)
            {
                return BusinessResult<FileDownloadResult>.Error("An unknown error occured while attempting to retrieve the file");
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

        private async Task<BusinessResult<FileDownloadResult>> DownloadRaw(string fileName, BlobClient client)
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

        private async Task<BlobContainerClient> GetContainerClient()
        {
            string containerName = "demo-blobstorage";
            
            BlobServiceClient client = new BlobServiceClient(ConnectionString);

            List<BlobContainerItem> containers = client.GetBlobContainers().ToList();
            
            // Create the container if it doesn't exist
            if (containers.FirstOrDefault(c => c.Name.Equals(containerName)) == null)
            {
                Response<BlobContainerClient> response = await client.CreateBlobContainerAsync(containerName);
                return response.Value;
            }

            return client.GetBlobContainerClient(containerName);
        }
    }
}