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
using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage.Blob;
using BlobProperties = Azure.Storage.Blobs.Models.BlobProperties;
using File = CloudDataProtection.Functions.BackupDemo.Entities.File;

namespace CloudDataProtection.Functions.BackupDemo.Business
{
    public class FileUploadBusinessLogic
    {
        private string ConnectionString => EnvironmentVariableHelper.GetEnvironmentVariable("CDP_DEMO_BLOB_CONNECTION");

        private readonly IFileTransformer _transformer;
        private readonly ITransformer _stringTransformer;

        private const int FilenameHashWorkFactor = 4;

        private const string FileNameKey = "original_name";

        public FileUploadBusinessLogic(IFileTransformer transformer, ITransformer stringTransformer)
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

            using (Stream stream = _transformer.Encrypt(input.OpenReadStream()))
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

        private string GetBlobName(IFormFile input)
        {
            string blobName = Guid.NewGuid() + "_" + input.FileName;

            string hash = BCrypt.Net.BCrypt.HashPassword(blobName, FilenameHashWorkFactor);

            char[] invalidChars = Path.GetInvalidFileNameChars().Append('.').ToArray();

            return string.Join("_", hash.Split(invalidChars, StringSplitOptions.RemoveEmptyEntries));
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