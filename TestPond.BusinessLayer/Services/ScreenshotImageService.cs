using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using TestPond.BusinessLayer.Services.CollectionRun;

namespace TestPond.BusinessLayer.Services
{
    public class ScreenshotImageService
    {
        private readonly IConfiguration _config;
        private readonly string _connectionString;

        private readonly string _blobContainerName = "screenshots";
        private readonly BlobContainerClient _blobContainerClient;

        public ScreenshotImageService(IConfiguration config)
        {
            _config = config;
            _connectionString = _config.GetValue<string>("ConnectionStrings:AzureBlobStorage");
            _blobContainerClient = new BlobContainerClient(_connectionString, _blobContainerName);
        }

        public async Task Get(string blobName) => throw new NotImplementedException();

        public async Task Save(Screenshot screenshot)
        {
            if (!_blobContainerClient.Exists())
            {
                await _blobContainerClient.CreateAsync();
            }

            using var ms = new MemoryStream();
            screenshot.File.CopyTo(ms);
            ms.Position = 0;

            //TODO: Implement "Folder Name"
            var blobPath = Path.Combine(screenshot.CollectionRunId.ToString(), screenshot.SingleDeviceRunId.ToString(), screenshot.File.FileName);
            BlobClient blobClient = _blobContainerClient.GetBlobClient(blobPath);
            BlobContentInfo blobInfo = await blobClient.UploadAsync(ms);
            var uri = blobClient.Uri;
            await ms.FlushAsync();
        }
    }
}
