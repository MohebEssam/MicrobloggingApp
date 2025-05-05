using Azure.Storage.Blobs;
using MicrobloggingApp.Infrastructure.Interfacses;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MicrobloggingApp.Infrastructure.Services
{
    public class AzureBlobService : IAzureBlobService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName = "posts";
        private readonly ILogger<AzureBlobService> _logger;

        public AzureBlobService(IConfiguration configuration, ILogger<AzureBlobService> logger)
        {
            _blobServiceClient = new BlobServiceClient(configuration["Azure:BlobStorageConnection"]);
            _logger = logger;
        }

        public Task ProcessAndResizeImageAsync(int PostId, string OriginalImageUrl)
        {
            throw new NotImplementedException();
        }

        public async Task<string> UploadImageAsync(IFormFile file, string blobName)
        {
            try
            {
                var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
                await containerClient.CreateIfNotExistsAsync();
                var blobClient = containerClient.GetBlobClient(blobName);

                using (var stream = file.OpenReadStream())
                {
                    await blobClient.UploadAsync(stream, overwrite: true);
                }

                return blobClient.Uri.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Uploading Images AzureBlob.");
                throw;
            }
        }
    }

}
