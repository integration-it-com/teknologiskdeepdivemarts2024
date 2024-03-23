using Azure.Storage.Blobs;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;

namespace deepdive
{
    public class MyFirstQueueFunction
    {
        private readonly ILogger<MyFirstQueueFunction> _logger;
        private readonly BlobHandler _blobHandler;

        public MyFirstQueueFunction(ILogger<MyFirstQueueFunction> logger)
        {
            _logger = logger;
            _blobHandler = new BlobHandler();
        }


        // To activate the trigger:
        // 1) Go to the Storage Account the connectionstring is from
        // 2) Go to Data Storages/Queues in the menu on the left hand side
        // 3) Create a new Queue called triggerqueue
        // 4) Under this queue, click Add message
        // 5) Under Message text, add (where upload.txt is a name of a file located under the container called input in the same Storage Account):
        // {
        //   "File": "upload.txt"
        // }
        // 6) This will trigger the QueueTrigger
        // 7) This will reverse the content of the file upload.txt and create a new blob in the "output" container in the same Storage Account.

        [Function(nameof(MyFirstQueueFunction))]
        public async Task Run([QueueTrigger("triggerqueue", Connection = "QueueStorageAccountConnectionString")] QueueMessage message)
        {
            _logger.LogInformation("C# Queue trigger function Processed processed message in queue");
            var newFileMetaData = JsonSerializer.Deserialize<AzureFunctionAppConfiguration>(message.Body);

            var contents = await _blobHandler.GetBlobAsync(newFileMetaData.File);
            string output = Reverse(contents);
            _logger.LogInformation("The reversed output was {Output}", output);
            string outputBlobName = DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".txt";
            await _blobHandler.SaveBlobAsync(output, outputBlobName);
        }


        private string Reverse(string input)
        {
            char[] reversedArray = input.ToCharArray();
            Array.Reverse(reversedArray);
            string output = new(reversedArray);
            return output;
        }
        private class AzureFunctionAppConfiguration
        {
            public string File { get; set; } = string.Empty;
        }


        // Warning: This is not good programming. In real world, use Dependency Injection or something else.
        private class BlobHandler
        {
            public async Task<string> GetBlobAsync(string blobName, string containerName = "input")
            {
                var client = GetBlobClient(blobName, containerName);
                var blobStream = await client.OpenReadAsync();

                StreamReader streamReader = new StreamReader(blobStream, Encoding.UTF8);

                string blob = await streamReader.ReadToEndAsync();
                return blob;
            }

            public async Task SaveBlobAsync(string contents, string blobName, string blobContainerName = "output")
            {
                BlobClient client = GetBlobClient(blobName, blobContainerName);

                var blobStream = new MemoryStream(Encoding.UTF8.GetBytes(contents));
                await client.UploadAsync(blobStream);
            }

            private static BlobClient GetBlobClient(string blobName, string blobContainerName)
            {
                // Better to use Managed Identity
                string connectionString = Environment.GetEnvironmentVariable("QueueStorageAccountConnectionString");
                var client = new BlobClient(connectionString, blobContainerName, blobName);
                return client;
            }
        }
    }
}
