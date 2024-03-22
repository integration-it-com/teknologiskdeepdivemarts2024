using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace deepdive
{
    public class MyFirstCSharpHttpFunction
    {
        private readonly ILogger _logger;

        public MyFirstCSharpHttpFunction(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<MyFirstCSharpHttpFunction>();
        }

        [Function("MyFirstCSharpHttpFunction")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString("Velkommen til Teknologisk Kursus!!!!!! I deep dive, nu med pipelines");

            return response;
        }
    }
}
