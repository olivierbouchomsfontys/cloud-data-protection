using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using CloudDataProtection.Core.Environment;
using CloudDataProtection.Functions.BackupDemo.Extensions;
using CloudDataProtection.Functions.BackupDemo.Service.Result;

namespace CloudDataProtection.Functions.BackupDemo.Service
{
    public class BlobStorageFileService : IFileService
    {
        private static string ConnectionString => EnvironmentVariableHelper.GetEnvironmentVariable("CDP_DEMO_BLOB_CONNECTION");

        private const string ContainerName = "cdp-demo-blobstorage";

        public async Task<UploadResult> Upload(Stream stream, string uploadFileName, IDictionary<string, string> tags)
        {
            BlobClient blobClient = await GetBlobClient(uploadFileName);

            Response<BlobContentInfo> response = await blobClient.UploadAsync(stream, new BlobUploadOptions { Tags = tags });

            if (!response.IsSuccessStatusCode())
            {
                return new UploadResult(false);
            }

            return new UploadResult(true) {Id = uploadFileName};
        }

        public async Task<InfoResult> GetInfo(string id)
        {
            BlobClient blobClient = await GetBlobClient(id);

            if (!await blobClient.ExistsAsync())
            {
                return new InfoResult(false) {IsNotFoundError = true};
            }

            Task<Response<BlobProperties>> properties = blobClient.GetPropertiesAsync();

            Task<Response<GetBlobTagResult>> tags = blobClient.GetTagsAsync();

            await Task.WhenAll(properties, tags);

            if (!properties.Result.IsSuccessStatusCode() || !tags.Result.IsSuccessStatusCode())
            {
                return new InfoResult(false);
            }

            return new InfoResult(true)
            {
                Bytes = properties.Result.Value.ContentLength,
                Tags = tags.Result.Value.Tags
            };
        }

        public async Task<Stream> Download(string id)
        {
            BlobClient blobClient = await GetBlobClient(id);

            BlobDownloadInfo response = await blobClient.DownloadAsync();

            return response.Content;
        }

        private async Task<BlobClient> GetBlobClient(string id)
        {
            BlobServiceClient client = new BlobServiceClient(ConnectionString);

            BlobContainerClient containerClient = client.GetBlobContainerClient(ContainerName);

            await containerClient.CreateIfNotExistsAsync();

            return containerClient.GetBlobClient(id);
        }
    }
}