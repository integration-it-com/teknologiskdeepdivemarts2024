using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace deepdive
{
    public class MyManagedIdentityBlobTriggerFunction
    {
        private readonly ILogger<MyBlobTriggerFunction> _logger;

        public MyManagedIdentityBlobTriggerFunction(ILogger<MyBlobTriggerFunction> logger)
        {
            _logger = logger;
        }


        [Function(nameof(MyManagedIdentityBlobTriggerFunction))]
        [BlobOutput("output/{rand-guid}.txt", Connection = "BlobStorageConnectionString")]
        public async Task<string> Run(
            [BlobTrigger("input/{name}", Connection = "BlobStorageConnectionString")] Stream stream,
            [BlobInput("input/config.json", Connection = "BlobStorageConnectionString")] Stream configStream,
            [BlobInput("input/config2.json", Connection = "BlobStorageConnectionString")] Stream configStream2,
            string name)
        {
            var config = JsonSerializer.Deserialize<AzureFunctionAppConfiguration>(configStream);
            var config2 = JsonSerializer.Deserialize<AzureFunctionAppConfiguration>(configStream2);

            using var blobStreamReader = new StreamReader(stream);
            var content = await blobStreamReader.ReadToEndAsync();
            _logger.LogInformation($"C# Blob trigger function Processed blob\n Name: {name} \n Data: {content}");
            string output = Reverse(content);

            return output;

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
            public string NewContent { get; set; } = string.Empty;
        }
    }
}
