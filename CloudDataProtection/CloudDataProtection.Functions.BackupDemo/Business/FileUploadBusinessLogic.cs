using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using CloudDataProtection.Core.Cryptography.Aes;
using CloudDataProtection.Core.Environment;
using CloudDataProtection.Core.Result;
using CloudDataProtection.Functions.BackupDemo.Extensions;
using Microsoft.AspNetCore.Http;

namespace CloudDataProtection.Functions.BackupDemo.Business
{
    public class FileUploadBusinessLogic
    {
        private string ConnectionString => EnvironmentVariableHelper.GetEnvironmentVariable("CDP_DEMO_BLOB_CONNECTION");

        private readonly IFileTransformer _transformer;

        private const int FilenameHashWorkFactor = 4;

        public FileUploadBusinessLogic(IFileTransformer transformer)
        {
            _transformer = transformer;
        }

        public async Task<BusinessResult<Entities.File>> Upload(IFormFile input)
        {
            BlobContainerClient client = await GetContainer();

            string blobName = GetBlobName(input);

            BlobClient blobClient = client.GetBlobClient(blobName);

            using (Stream stream = _transformer.Encrypt(input.OpenReadStream()))
            {
                Response<BlobContentInfo> response = await blobClient.UploadAsync(stream);

                Entities.File file = new Entities.File
                {
                    StorageId = blobName
                };

                if (response.IsSuccessStatusCode())
                {
                    return BusinessResult<Entities.File>.Ok(file);
                }

                return BusinessResult<Entities.File>.Error("An unknown error occured while processing the file.");
            }      
        }

        private string GetBlobName(IFormFile input)
        {
            string blobName = Guid.NewGuid() + "_" + input.FileName;

            string hash = BCrypt.Net.BCrypt.HashPassword(blobName, FilenameHashWorkFactor);

            char[] invalidChars = Path.GetInvalidFileNameChars().Append('.').ToArray();

            return string.Join("_", hash.Split(invalidChars, StringSplitOptions.RemoveEmptyEntries));
        }

        private async Task<BlobContainerClient> GetContainer()
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