using Azure.Storage.Queues;
using MicrobloggingApp.Infrastructure.Interfacses;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text;

namespace MicrobloggingApp.Infrastructure.Services
{
    public class QueueService : IQueueService
    {
        private readonly QueueClient _queueClient;
        private readonly ILogger<QueueService> _logger;

        public QueueService(IConfiguration config, ILogger<QueueService> logger)
        {
            _queueClient = new QueueClient(config["Azure:QueueConnection"], "image-processing-queue");
            _logger = logger;

            // uncomment this when needed
            //_queueClient.CreateIfNotExists();
        }

        public async Task SendMessageAsync(string message)
        {
            try
            {
                await _queueClient.SendMessageAsync(Convert.ToBase64String(Encoding.UTF8.GetBytes(message)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in sending messages to the queue.");
                throw;
            }

        }
    }

}
