using Azure.Storage.Queues;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyFoodDoc.App.Infrastructure.Azure.Queue.Abstractions;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.App.Infrastructure.Azure.Queue
{
    public class QueueService : IQueueService
    {
        private readonly string _connectionString;
        private readonly ILogger _logger;

        public QueueService(IConfiguration configuration, ILogger<QueueService> logger)
        {
            _connectionString = configuration.GetConnectionString("BlobStorageConnection");
            _logger = logger;
        }

        public async Task AddMessage(string queueName, string message, CancellationToken cancellationToken = default)
        {
            // Instantiate a QueueClient which will be used to create and manipulate the queue
            QueueClient queueClient = new QueueClient(
                _connectionString,
                queueName,
                new QueueClientOptions
                {
                    MessageEncoding = QueueMessageEncoding.Base64
                });


            // Create the queue
            var responseQueue = await queueClient.CreateIfNotExistsAsync();

            if (responseQueue?.Status == 201)
            {
                _logger.LogInformation($"Queue {queueName} created.");
            }

            var exists = await queueClient.ExistsAsync();

            if (exists?.Value == false)
            {
                _logger.LogError($"Could not create / use Queue {queueName}.");
                return;
            }

            // Send a message to the queue
            var responseMessage = await queueClient.SendMessageAsync(message, cancellationToken);

            if (responseMessage?.Value is null)
            {
                _logger.LogError($"Could not send message {message} to queue {queueName}. response {responseMessage?.GetRawResponse()}");

            }
            else
            {
                _logger.LogInformation($"Message {message} sent to queue {queueName}. Response {JsonSerializer.Serialize(responseMessage?.Value)}");
            }
        }

        public async Task AddMessage<T>(string queueName, T message, CancellationToken cancellationToken = default)
        {

            var serializedMessage = JsonSerializer.Serialize(
                message,
                new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                    WriteIndented = true
                });

            await AddMessage(queueName, serializedMessage, cancellationToken);
        }
    }
}
