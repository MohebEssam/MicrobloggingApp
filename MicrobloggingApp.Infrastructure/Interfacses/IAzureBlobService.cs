using Microsoft.AspNetCore.Http;

namespace MicrobloggingApp.Infrastructure.Interfacses
{
    public interface IAzureBlobService
    {
        Task<string> UploadImageAsync(IFormFile file, string blobName);
        Task ProcessAndResizeImageAsync(int PostId, string OriginalImageUrl);
    }
}
