using MicrobloggingApp.Infrastructure.Interfacses;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace MicrobloggingAppFunctions
{
    public class ProcessImageFunction
    {
        private readonly IAzureBlobService _blobService;

        public ProcessImageFunction(IAzureBlobService blobService)
        {
            _blobService = blobService;
        }

        [FunctionName("ProcessImageFunction")]
        public async Task RunAsync(
            [QueueTrigger("your-queue-name", Connection = "AzureWebJobsStorage")] string queueItem,
            ILogger log)
        {
            var message = JsonConvert.DeserializeObject<ImageProcessMessage>(queueItem);

            await _blobService.ProcessAndResizeImageAsync(message.PostId, message.OriginalImageUrl);

            log.LogInformation($"Processed image for PostId: {message.PostId}");
        }
    }

    public class ImageProcessMessage
    {
        public int PostId { get; set; }
        public string OriginalImageUrl { get; set; }
    }

}
