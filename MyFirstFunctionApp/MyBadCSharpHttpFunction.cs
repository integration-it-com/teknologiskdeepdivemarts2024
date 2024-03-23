using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace deepdive
{
    public class MyBadCSharpHttpFunction
    {
        private readonly ILogger _logger;

        public MyBadCSharpHttpFunction(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<MyFirstCSharpHttpFunction>();
        }

        [Function("MyBadCSharpHttpFunction")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request. This will not work!!!!");
            throw new Exception("The Function App crashed");
        }
    }
}
